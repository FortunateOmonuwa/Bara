using Hangfire;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.BackgroudServices;
using Services.FileStorageServices.Interfaces;
using Services.MailingService;
using Services.YouVerifyIntegration;
using Shared.Models;
using SharedModule.DTOs.AddressDTOs;
using SharedModule.Models;
using SharedModule.Settings;
using SharedModule.Utils;
using System.Security.Cryptography;
using TransactionModule.DTOs;
using TransactionModule.Models;
using UserModule.DTOs.ServiceDTOs;
using UserModule.DTOs.WriterDTOs;
using UserModule.Enums;
using UserModule.Interfaces.UserInterfaces;
using UserModule.Models;
using UserModule.Utilities;
using Role = UserModule.Enums.Role;

namespace Infrastructure.Repositories.UserRepositories
{
    public class WriterRepository : IWriterService
    {
        private readonly BaraContext dbContext;
        private readonly ILogger<WriterRepository> logger;
        private readonly AppSettings settings;
        private readonly IFileService fileService;
        private readonly HangfireJobs hangfire;
        private readonly IMemoryCache cache;
        private readonly IYouVerifyService youVerify;
        public WriterRepository(BaraContext dbContext, ILogger<WriterRepository> logger, IOptions<AppSettings> appSettings,
            IFileService fileService, HangfireJobs hangfireJobs, IMemoryCache memoryCache, IYouVerifyService youVerifyService)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            settings = appSettings.Value;
            this.fileService = fileService;
            hangfire = hangfireJobs;
            cache = memoryCache;
            youVerify = youVerifyService;
        }

        public async Task<ResponseDetail<GetWriterDetailDTO>> AddWriter(PostWriterDetailDTO writerDetailDTO)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                // -------------------- INPUT VALIDATION --------------------
                var email = writerDetailDTO.Email.Trim().ToLowerInvariant();
                var emailValidation = RegexValidations.IsValidMail(email);
                var phoneNumberValidation = RegexValidations.IsValidPhoneNumber(writerDetailDTO.PhoneNumber);
                var nameValidation = RegexValidations.IsValidName(writerDetailDTO.FirstName, writerDetailDTO.LastName, writerDetailDTO.MiddleName ?? "");
                var passwordValidation = RegexValidations.IsAcceptablePasswordFormat(writerDetailDTO.Password);
                var validationErrors = new List<string>();

                if (!emailValidation) validationErrors.Add("Invalid email");
                if (!phoneNumberValidation) validationErrors.Add("Invalid phone number");
                if (!nameValidation) validationErrors.Add("Names can only contain alphabets");
                if (!passwordValidation) validationErrors.Add("Password must be strong");

                if (validationErrors.Count > 0)
                {
                    return ResponseDetail<GetWriterDetailDTO>.Failed(string.Join(" | ", validationErrors));
                }

                // -------------------- CHECK FOR EXISTING ACCOUNT --------------------
                var writerAccount = await dbContext.AuthProfiles
                    .Select(x => new { x.Email, x.IsDeleted })
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (writerAccount?.IsDeleted == true)
                {
                    return ResponseDetail<GetWriterDetailDTO>.Failed("A profile with this email already exists and just needs to be Reactivated", 409, "Account Needs Reactivation");
                }
                else if (writerAccount is not null)
                {
                    return ResponseDetail<GetWriterDetailDTO>.Failed("A profile with this email already exists...", 409, "Conflict");
                }

                // -------------------- CREATE WRITER PROFILE --------------------
                var newWriterProfile = new Writer
                {
                    Email = email,
                    FirstName = writerDetailDTO.FirstName.ToUpperInvariant(),
                    LastName = writerDetailDTO.LastName.ToUpperInvariant(),
                    MiddleName = writerDetailDTO.MiddleName?.ToUpperInvariant() ?? "",
                    PhoneNumber = writerDetailDTO.PhoneNumber,
                    Bio = writerDetailDTO.Bio,
                    Gender = writerDetailDTO.Gender,
                    DateOfBirth = writerDetailDTO.DateOfBirth,
                    IsPremiumMember = writerDetailDTO.IsPremiumMember,
                    Address = new Address
                    {
                        City = writerDetailDTO.AddressDetail.City.ToUpperInvariant(),
                        Country = writerDetailDTO.AddressDetail.Country.ToUpperInvariant(),
                        State = writerDetailDTO.AddressDetail.State.ToUpperInvariant(),
                        Street = writerDetailDTO.AddressDetail.Street.ToUpperInvariant(),
                        PostalCode = writerDetailDTO.AddressDetail.PostalCode,
                        AdditionalDetails = writerDetailDTO.AddressDetail.AdditionalDetails.ToUpperInvariant(),
                    },
                    Type = Role.Writer,
                };

                // -------------------- CREATE AUTH PROFILE --------------------
                AuthProfile authProfile = new()
                {
                    Email = email,
                    Password = BCrypt.Net.BCrypt.HashPassword(writerDetailDTO.Password),
                    Role = "Writer",
                    FullName = $"{writerDetailDTO.FirstName} {writerDetailDTO.LastName}".ToUpperInvariant(),
                    UserId = newWriterProfile.Id
                };

                // -------------------- CREATE SERVICES --------------------
                var services = writerDetailDTO.PostServiceDetail?
                    .Select(dto => new Service
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        MinPrice = dto.MinPrice,
                        MaxPrice = dto.MaxPrice,
                        Currency = dto.Currency,
                        IPDealType = dto.IPDealType,
                        SharePercentage = dto.SharePercentage,
                        PaymentType = dto.PaymentType,
                        Genre = dto.Genre ?? [],
                        WriterId = newWriterProfile.Id
                    })
                    .ToList() ?? [];

                // -------------------- CREATE WALLET --------------------
                Wallet wallet = new Wallet
                {
                    TotalBalance = 0,
                    AvailableBalance = 0,
                    LockedBalance = 0,
                    Currency = Currency.NAIRA,
                    UserId = newWriterProfile.Id
                };

                newWriterProfile.AuthProfile = authProfile;
                newWriterProfile.Services = services;
                newWriterProfile.Wallet = wallet;

                // --------------------  UPLOAD & ASSIGN DOCUMENT ID --------------------
                var userDirectoryName = $"Writer_{newWriterProfile.FirstName}_{newWriterProfile.LastName}-{newWriterProfile.PhoneNumber}";
                var documentId = await fileService.ProcessDocumentForUpload(userDirectoryName, writerDetailDTO.VerificationDocument);
                if (documentId == null || documentId.Data == Guid.Empty)
                {
                    logger.LogError($"An error occurred while uploading KYC document for {writerDetailDTO.FirstName} {writerDetailDTO.LastName}");
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"An error occurred while uploading KYC document for {writerDetailDTO.FirstName} {writerDetailDTO.LastName}", 500, "Unexpected Error");
                }
                newWriterProfile.DocumentID = documentId.Data;

                // -------------------- SAVE PROFILE TO DATABASE --------------------
                await dbContext.Writers.AddAsync(newWriterProfile);
                var writerRes = await dbContext.SaveChangesAsync();
                if (writerRes < 1)
                {
                    logger.LogError($"An error occurred while creating a writer profile for {writerDetailDTO.FirstName} {writerDetailDTO.LastName}");
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"An error occurred while creating a writer profile for {writerDetailDTO.FirstName} {writerDetailDTO.LastName}", 500, "Unexpected Error");
                }

                // --------------------  GENERATE EMAIL VERIFICATION TOKEN --------------------
                var token = RandomNumberGenerator.GetInt32(100000, 999999);
                cache.Set($"User_Verification_Token_{newWriterProfile.Id}", token, absoluteExpiration: DateTimeOffset.UtcNow.AddMinutes(10));
                Console.WriteLine($"Writer_Verification_Token_{writerDetailDTO.FirstName} {writerDetailDTO.LastName}", token);

                var verificationMail = MailNotifications.RegistrationConfirmationMailNotification(newWriterProfile.Email, newWriterProfile.FirstName, token.ToString());
                BackgroundJob.Enqueue<HangfireJobs>(x => x.SendMailAsync(verificationMail));

                // --------------------  PREPARE KYC REQUEST --------------------
                var kycDetail = new YouVerifyKycDto
                {
                    Id = writerDetailDTO.VerificationDocument.VerificationNumber,
                    Type = writerDetailDTO.VerificationDocument.Type.ToString(),
                    UserId = newWriterProfile.Id,
                    LastName = newWriterProfile.LastName,
                    IsDirectCall = true,
                };

                // --------------------  BUILD RESPONSE DTO --------------------
                var writerProfile = new GetWriterDetailDTO
                {
                    Id = newWriterProfile.Id,
                    Email = newWriterProfile.Email,
                    FirstName = newWriterProfile.FirstName,
                    LastName = newWriterProfile.LastName,
                    MiddleName = newWriterProfile.MiddleName,
                    Name = $"{newWriterProfile.FirstName} {newWriterProfile.LastName}",
                    Bio = newWriterProfile.Bio,
                    IsBlacklisted = newWriterProfile.AuthProfile.IsDeleted,
                    Address = new AddressDetail
                    {
                        City = newWriterProfile.Address.City,
                        Country = newWriterProfile.Address.Country,
                        State = newWriterProfile.Address.State,
                        Street = newWriterProfile.Address.Street,
                        PostalCode = newWriterProfile.Address.PostalCode,
                        AdditionalDetails = newWriterProfile.Address.AdditionalDetails,
                    },
                    IsEmailVerified = newWriterProfile.AuthProfile.IsEmailVerified,
                    IsVerified = newWriterProfile.AuthProfile.IsVerified,
                    PhoneNumber = newWriterProfile.PhoneNumber,
                    VerificationStatus = newWriterProfile.VerificationStatus.ToString(),
                    Role = newWriterProfile.AuthProfile.Role,
                    IsPremium = newWriterProfile.IsPremiumMember,
                    Services = newWriterProfile.Services.Select(s => new GetServiceDetailDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        MinPrice = s.MinPrice,
                        MaxPrice = s.MaxPrice,
                        Currency = s.Currency,
                        IPDealType = s.IPDealType,
                        SharePercentage = s.SharePercentage,
                        PaymentType = s.PaymentType,
                        Genre = s.Genre,
                    }).ToList(),
                    Wallet = new GetWalletDetailDTO
                    {
                        Id = newWriterProfile.Wallet.Id,
                        Balance = newWriterProfile.Wallet.TotalBalance,
                        LockedBalance = newWriterProfile.Wallet.LockedBalance,
                        Currency = newWriterProfile.Wallet.Currency,
                        CurrencySymbol = newWriterProfile.Wallet.CurrencySymbol,
                        UserId = newWriterProfile.Id
                    },
                    CreatedAt = newWriterProfile.CreatedAt,
                    DateCreated = newWriterProfile.DateCreated,
                    TimeCreated = newWriterProfile.TimeCreated,
                    DateModified = newWriterProfile.DateModified,
                    ModifiedAt = newWriterProfile.ModifiedAt,
                    TimeModified = newWriterProfile.TimeModified
                };

                // -------------------- PERFORM DIRECT KYC CHECK --------------------
                bool kycCompleted = false;
                string kycFailureReason = null;
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    try
                    {
                        var directKycReq = await youVerify.VerifyIdentificationNumberAsync(kycDetail, cts.Token);

                        if (directKycReq != null && directKycReq.Success && directKycReq.Data?.Id != null)
                        {
                            if (directKycReq.Data?.DateOfBirth != newWriterProfile.DateOfBirth.ToString("yyyy-MM-dd"))
                            {
                                logger.LogWarning($"Writer {newWriterProfile.FirstName} {newWriterProfile.LastName} KYC failed: Date of birth mismatch.");
                                kycFailureReason = "DOB_MISMATCH";
                            }
                            else
                            {
                                newWriterProfile.AuthProfile.IsVerified = true;
                                newWriterProfile.VerificationStatus = VerificationStatus.Approved;
                                dbContext.Writers.Update(newWriterProfile);
                                await dbContext.SaveChangesAsync();
                                kycCompleted = true;
                            }
                        }
                        else
                        {
                            kycFailureReason = "KYC_FAILED";
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        kycFailureReason = "TIMEOUT";
                        logger.LogWarning($"YouVerify KYC call timed out after 30 seconds for writer {newWriterProfile.Id}. Falling back to background job.");
                    }
                    catch (Exception ex)
                    {
                        kycFailureReason = "ERROR";
                        logger.LogError(ex.GetType().Name, $"Error during YouVerify KYC call for writer {newWriterProfile.Id}. Falling back to background job.");
                    }
                }

                // --------------------  COMMIT TRANSACTION --------------------
                await transaction.CommitAsync();

                // -------------------- HANDLE KYC OUTCOME --------------------
                string message;
                if (!kycCompleted)
                {
                    // ------------------------ LET HANGFIRE HANDLE AS A BACKGROUND JOB IF IT FAILS -------------------------
                    BackgroundJob.Enqueue(() => hangfire.StartKycProcess(kycDetail));

                    message = kycFailureReason switch
                    {
                        "DOB_MISMATCH" => $"Writer profile created successfully... But verification failed because of a mismatch between the Date of birth provided and {writerDetailDTO.VerificationDocument.Type} Date of birth",
                        "TIMEOUT" => "Writer profile created successfully... But verification could not be completed within the time limit. We will continue processing in the background.",
                        "ERROR" => "Writer profile created successfully... But an error occurred during verification. We will retry shortly.",
                        _ => "Writer profile created successfully... But verification could not be completed at this time."
                    };
                }
                else
                {
                    message = "Writer profile created successfully";
                }

                // -------------------- CACHE FINAL PROFILE --------------------
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                cache.Set($"Writer_Profile{newWriterProfile.Id}", writerProfile, cacheOptions);

                // -------------------- RETURN SUCCESS --------------------
                return ResponseDetail<GetWriterDetailDTO>.Successful(writerProfile, message, 201);
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                logger.LogError($"A database update exception: {dbEx.GetType().Name} was thrown while creating an account for {writerDetailDTO.FirstName} {writerDetailDTO.LastName}... Base Exception: {dbEx.GetBaseException().GetType()}", $"Exception Code: {dbEx.HResult}");
                return ResponseDetail<GetWriterDetailDTO>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError($"An exception: {ex.GetType().Name} was thrown while creating a writer profile... \nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}", ex.Message);
                return ResponseDetail<GetWriterDetailDTO>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }
        public Task<ResponseDetail<bool>> DeleteWriter(Guid writerId)
        {
            //Set as deleted and delete login profile
            throw new NotImplementedException();
        }

        public async Task<ResponseDetail<GetWriterDetailDTO>> GetWriterDetail(Guid writerId)
        {
            try
            {
                var writerProfile = await dbContext.Writers.Where(x => x.AuthProfile.IsDeleted == false).
                                    Select(x => new GetWriterDetailDTO
                                    {
                                        Id = x.Id,
                                        Email = x.Email,
                                        FirstName = x.FirstName,
                                        LastName = x.LastName,
                                        Name = $"{x.FirstName} {x.LastName}",
                                        Bio = x.Bio,
                                        IsPremium = x.IsPremiumMember,
                                        MiddleName = x.MiddleName ?? "-",
                                        Services = x.Services.Select(x => new GetServiceDetailDTO
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Currency = x.Currency,
                                            CurrencySymbol = x.CurrencySymbol,
                                            Description = x.Description,
                                            Genre = x.Genre,
                                            IPDealType = x.IPDealType,
                                            MaxPrice = x.MaxPrice,
                                            MinPrice = x.MinPrice,
                                            PaymentType = x.PaymentType,
                                            SharePercentage = x.SharePercentage
                                        }).ToList(),
                                        Address = new AddressDetail
                                        {
                                            City = x.Address.City,
                                            Country = x.Address.Country,
                                            State = x.Address.State,
                                            Street = x.Address.Street,
                                            AdditionalDetails = x.Address.AdditionalDetails ?? "-",
                                            PostalCode = x.Address.PostalCode ?? "-",
                                        },
                                        Role = x.AuthProfile.Role,
                                        Wallet = new GetWalletDetailDTO
                                        {
                                            Balance = x.Wallet.TotalBalance,
                                            Currency = x.Wallet.Currency,
                                            CurrencySymbol = x.Wallet.CurrencySymbol,
                                            LockedBalance = x.Wallet.LockedBalance,
                                            Id = x.Wallet.Id,
                                            UserId = x.Id
                                        },
                                        IsBlacklisted = x.AuthProfile.IsDeleted,
                                        CreatedAt = x.CreatedAt,
                                        DateCreated = x.DateCreated,
                                        TimeCreated = x.TimeCreated,
                                        DateModified = x.DateModified,
                                        IsEmailVerified = x.AuthProfile.IsEmailVerified,
                                        IsVerified = x.AuthProfile.IsVerified,
                                        ModifiedAt = x.ModifiedAt,
                                        PhoneNumber = x.PhoneNumber,
                                        TimeModified = x.TimeModified,
                                        VerificationStatus = x.VerificationStatus.ToString()
                                    }).FirstOrDefaultAsync(x => x.Id == writerId);
                if (writerProfile == null)
                {
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"Writer with ID:{writerId} does not exist", 404, "Not Found");
                }
                return ResponseDetail<GetWriterDetailDTO>.Successful(writerProfile);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.GetType().FullName} was thrown while fetching writer profile...\n Base Exception: {ex.GetBaseException().GetType().FullName}", ex.Message);
                return ResponseDetail<GetWriterDetailDTO>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public Task<ResponseDetail<List<GetWriterDetailDTO>>> GetWriters(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<GetWriterDetailDTO>> UpdateWriterDetail(Guid writerId, PostWriterDetailDTO updateWriterDetail)
        {
            throw new NotImplementedException();
        }
    }
}