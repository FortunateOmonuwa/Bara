using Hangfire;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Services.BackgroudServices;
using Services.FileStorageServices.Interfaces;
using Services.MailingService;
using Services.YouVerifyIntegration;
using Shared.Models;
using SharedModule.DTOs.AddressDTOs;
using SharedModule.Models;
using SharedModule.Utils;
using System.Security.Cryptography;
using TransactionModule.DTOs;
using TransactionModule.Models;
using UserModule.DTOs.ProducerDTOs;
using UserModule.Enums;
using UserModule.Interfaces.UserInterfaces;
using UserModule.Models;
using UserModule.Utilities;

namespace Infrastructure.Repositories.UserRepositories
{
    public class ProducerRepository : IProducerService
    {
        private readonly BaraContext dbContext;
        private readonly ILogger<WriterRepository> logger;
        private readonly IFileService fileService;
        private readonly HangfireJobs hangfire;
        private readonly IMemoryCache cache;
        private readonly IYouVerifyService youVerify;
        public ProducerRepository(BaraContext baraContext, ILogger<WriterRepository> logger, IFileService fileService,
            HangfireJobs hangfireJobs, IMemoryCache memoryCache, IYouVerifyService youVerify)
        {
            dbContext = baraContext;
            this.logger = logger;
            this.fileService = fileService;
            hangfire = hangfireJobs;
            cache = memoryCache;
            this.youVerify = youVerify;
        }
        public async Task<ResponseDetail<GetProducerDetailDTO>> AddProducer(PostProducerDetailDTO producerDetailDTO)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                // -------------------- INPUT VALIDATION --------------------
                var email = producerDetailDTO.Email.Trim().ToLowerInvariant();
                var emailValidation = RegexValidations.IsValidMail(email);
                var phoneNumberValidation = RegexValidations.IsValidPhoneNumber(producerDetailDTO.PhoneNumber);
                var nameValidation = RegexValidations.IsValidName(producerDetailDTO.FirstName, producerDetailDTO.LastName, producerDetailDTO.MiddleName ?? "");
                var passwordValidation = RegexValidations.IsAcceptablePasswordFormat(producerDetailDTO.Password);
                var validationErrors = new List<string>();

                if (!emailValidation) validationErrors.Add("Invalid email");
                if (!phoneNumberValidation) validationErrors.Add("Invalid phone number");
                if (!nameValidation) validationErrors.Add("Names can only contain alphabets");
                if (!passwordValidation) validationErrors.Add("Password must be strong");

                if (validationErrors.Count > 0)
                {
                    return ResponseDetail<GetProducerDetailDTO>.Failed(string.Join(" | ", validationErrors));
                }

                // -------------------- CHECK FOR EXISTING ACCOUNT --------------------
                var producerAccount = await dbContext.AuthProfiles
                    .Select(x => new { x.Email, x.IsDeleted })
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (producerAccount?.IsDeleted == true)
                {
                    return ResponseDetail<GetProducerDetailDTO>.Failed("A profile with this email already exists and just needs to be Reactivated", 409, "Account Needs Reactivation");
                }
                else if (producerAccount is not null)
                {
                    return ResponseDetail<GetProducerDetailDTO>.Failed("A profile with this email already exists...", 409, "Conflict");
                }

                // -------------------- CREATE PRODUCER PROFILE --------------------
                var newProducerProfile = new Producer
                {
                    Email = email,
                    FirstName = producerDetailDTO.FirstName.ToUpperInvariant(),
                    LastName = producerDetailDTO.LastName.ToUpperInvariant(),
                    MiddleName = producerDetailDTO.MiddleName?.ToUpperInvariant() ?? "",
                    PhoneNumber = producerDetailDTO.PhoneNumber,
                    Bio = producerDetailDTO.Bio,
                    Gender = producerDetailDTO.Gender,
                    DateOfBirth = producerDetailDTO.DateOfBirth,
                    Address = new Address
                    {
                        City = producerDetailDTO.AddressDetail.City.ToUpperInvariant(),
                        Country = producerDetailDTO.AddressDetail.Country.ToUpperInvariant(),
                        State = producerDetailDTO.AddressDetail.State.ToUpperInvariant(),
                        Street = producerDetailDTO.AddressDetail.Street.ToUpperInvariant(),
                        PostalCode = producerDetailDTO.AddressDetail.PostalCode,
                        AdditionalDetails = producerDetailDTO.AddressDetail.AdditionalDetails.ToUpperInvariant(),
                    },
                    Type = Role.Producer,
                };

                // -------------------- CREATE AUTH PROFILE --------------------
                AuthProfile authProfile = new()
                {
                    Email = email,
                    Password = BCrypt.Net.BCrypt.HashPassword(producerDetailDTO.Password),
                    Role = "Producer",
                    FullName = $"{producerDetailDTO.FirstName} {producerDetailDTO.LastName}".ToUpperInvariant(),
                    UserId = newProducerProfile.Id
                };

                // -------------------- CREATE WALLET --------------------
                Wallet wallet = new Wallet
                {
                    TotalBalance = 0,
                    AvailableBalance = 0,
                    LockedBalance = 0,
                    Currency = Currency.NAIRA,
                    UserId = newProducerProfile.Id
                };

                newProducerProfile.AuthProfile = authProfile;
                newProducerProfile.Wallet = wallet;

                // --------------------  UPLOAD & ASSIGN DOCUMENT ID --------------------
                var userDirectoryName = $"Producer_{newProducerProfile.FirstName}_{newProducerProfile.LastName}-{newProducerProfile.PhoneNumber}";
                var documentId = await fileService.ProcessDocumentForUpload(userDirectoryName, producerDetailDTO.VerificationDocument);
                if (documentId == null || documentId.Data == Guid.Empty)
                {
                    logger.LogError($"An error occurred while uploading KYC document for {producerDetailDTO.FirstName} {producerDetailDTO.LastName}");
                    return ResponseDetail<GetProducerDetailDTO>.Failed($"An error occurred while uploading KYC document for {producerDetailDTO.FirstName} {producerDetailDTO.LastName}", 500, "Unexpected Error");
                }
                newProducerProfile.DocumentID = documentId.Data;

                // -------------------- SAVE PROFILE TO DATABASE --------------------
                await dbContext.Producers.AddAsync(newProducerProfile);
                var producerRes = await dbContext.SaveChangesAsync();
                if (producerRes < 1)
                {
                    logger.LogError($"An error occurred while creating a producer profile for {producerDetailDTO.FirstName} {producerDetailDTO.LastName}");
                    return ResponseDetail<GetProducerDetailDTO>.Failed($"An error occurred while creating a producer profile for {producerDetailDTO.FirstName} {producerDetailDTO.LastName}", 500, "Unexpected Error");
                }

                // --------------------  GENERATE EMAIL VERIFICATION TOKEN --------------------
                var token = RandomNumberGenerator.GetInt32(100000, 999999);
                cache.Set($"User_Verification_Token_{newProducerProfile.Id}", token, absoluteExpiration: DateTimeOffset.UtcNow.AddMinutes(10));
                Console.WriteLine($"Producer_Verification_Token_{producerDetailDTO.FirstName} {producerDetailDTO.LastName}", token);

                var verificationMail = MailNotifications.RegistrationConfirmationMailNotification(newProducerProfile.Email, newProducerProfile.FirstName, token.ToString());
                BackgroundJob.Enqueue<HangfireJobs>(x => x.SendMailAsync(verificationMail));

                // --------------------  PREPARE KYC REQUEST --------------------
                var kycDetail = new YouVerifyKycDto
                {
                    Id = producerDetailDTO.VerificationDocument.VerificationNumber,
                    Type = producerDetailDTO.VerificationDocument.Type.ToString(),
                    UserId = newProducerProfile.Id,
                    LastName = newProducerProfile.LastName,
                };

                // --------------------  BUILD RESPONSE DTO --------------------
                var producerProfile = new GetProducerDetailDTO
                {
                    Id = newProducerProfile.Id,
                    Email = newProducerProfile.Email,
                    FirstName = newProducerProfile.FirstName,
                    LastName = newProducerProfile.LastName,
                    MiddleName = newProducerProfile.MiddleName,
                    Name = $"{newProducerProfile.FirstName} {newProducerProfile.LastName}",
                    Bio = newProducerProfile.Bio,
                    IsBlacklisted = newProducerProfile.AuthProfile.IsDeleted,
                    Address = new AddressDetail
                    {
                        City = newProducerProfile.Address.City,
                        Country = newProducerProfile.Address.Country,
                        State = newProducerProfile.Address.State,
                        Street = newProducerProfile.Address.Street,
                        PostalCode = newProducerProfile.Address.PostalCode,
                        AdditionalDetails = newProducerProfile.Address.AdditionalDetails,
                    },
                    IsEmailVerified = newProducerProfile.AuthProfile.IsEmailVerified,
                    IsVerified = newProducerProfile.AuthProfile.IsVerified,
                    PhoneNumber = newProducerProfile.PhoneNumber,
                    VerificationStatus = newProducerProfile.VerificationStatus
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
                            if (directKycReq.Data?.DateOfBirth != newProducerProfile.DateOfBirth.ToString("yyyy-MM-dd"))
                            {
                                logger.LogWarning($"Producer {newProducerProfile.FirstName} {newProducerProfile.LastName} KYC failed: Date of birth mismatch.");
                                kycFailureReason = "DOB_MISMATCH";
                            }
                            else
                            {
                                newProducerProfile.AuthProfile.IsVerified = true;
                                newProducerProfile.VerificationStatus = VerificationStatus.Approved;
                                dbContext.Producers.Update(newProducerProfile);
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
                        logger.LogWarning($"YouVerify KYC call timed out after 30 seconds for producer {newProducerProfile.Id}. Falling back to background job.");
                    }
                    catch (Exception ex)
                    {
                        kycFailureReason = "ERROR";
                        logger.LogError(ex.GetType().Name, $"Error during YouVerify KYC call for producer {newProducerProfile.Id}. Falling back to background job.");
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
                        "DOB_MISMATCH" => $"Producer profile created successfully... But verification failed because of a mismatch between the Date of birth provided and {producerDetailDTO.VerificationDocument.Type} Date of birth",
                        "TIMEOUT" => "Producer profile created successfully... But verification could not be completed within the time limit. We will continue processing in the background.",
                        "ERROR" => "Producer profile created successfully... But an error occurred during verification. We will retry shortly.",
                        _ => "Producer profile created successfully... But verification could not be completed at this time."
                    };
                }
                else
                {
                    message = "Producer profile created successfully";
                }

                // -------------------- CACHE FINAL PROFILE --------------------
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                cache.Set($"Producer_Profile{newProducerProfile.Id}", producerProfile, cacheOptions);

                // -------------------- RETURN SUCCESS --------------------
                return ResponseDetail<GetProducerDetailDTO>.Successful(producerProfile, message, 201);
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                logger.LogError($"A database update exception: {dbEx.GetType().Name} was thrown while creating an account for {producerDetailDTO.FirstName} {producerDetailDTO.LastName}... Base Exception: {dbEx.GetBaseException().GetType()}", $"Exception Code: {dbEx.HResult}");
                return ResponseDetail<GetProducerDetailDTO>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError($"An exception: {ex.GetType().Name} was thrown while creating a producer profile... \nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}", ex.Message);
                return ResponseDetail<GetProducerDetailDTO>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public Task<bool> DeleteProducer(Guid producerId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDetail<GetProducerDetailDTO>> GetProducer(Guid producerId)
        {
            try
            {
                var producer = await dbContext.Producers.Where(x => x.AuthProfile.IsDeleted == false).
                                    Select(x => new GetProducerDetailDTO
                                    {
                                        Id = x.Id,
                                        Email = x.Email,
                                        FirstName = x.FirstName,
                                        LastName = x.LastName,
                                        Name = $"{x.FirstName} {x.LastName}",
                                        Bio = x.Bio,
                                        MiddleName = x.MiddleName ?? "-",
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
                                        IsEmailVerified = x.AuthProfile.IsDeleted,
                                        IsVerified = x.AuthProfile.IsDeleted,
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
                                        ModifiedAt = x.ModifiedAt,
                                        PhoneNumber = x.PhoneNumber,
                                        TimeModified = x.TimeModified,
                                        VerificationStatus = x.VerificationStatus,
                                    }).FirstOrDefaultAsync(x => x.Id == producerId);
                if (producer is null)
                {
                    return ResponseDetail<GetProducerDetailDTO>.Failed($"Producer with ID:{producerId} does not exist", 404, "Not Found");
                }
                return ResponseDetail<GetProducerDetailDTO>.Successful(producer);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception {ex.GetType().FullName} was thrown while fetching producer's profile...\n Base Exception: {ex.GetBaseException().GetType().FullName}", ex.Message);
                return ResponseDetail<GetProducerDetailDTO>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public Task<ResponseDetail<GetProducerDetailDTO>> UpdateProducer(PostProducerDetailDTO producerDetailDTO, Guid producerId)
        {
            throw new NotImplementedException();
        }
    }
}
