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

        public WriterRepository(BaraContext dbContext, ILogger<WriterRepository> logger, IOptions<AppSettings> appSettings, IFileService fileService, HangfireJobs hangfireJobs, IMemoryCache memoryCache)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            settings = appSettings.Value;
            this.fileService = fileService;
            hangfire = hangfireJobs;
            cache = memoryCache;
        }

        public async Task<ResponseDetail<GetWriterDetailDTO>> AddWriter(PostWriterDetailDTO writerDetail)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var email = writerDetail.Email.Trim().ToLowerInvariant();
                var emailValidation = RegexValidations.IsValidMail(email);
                var phoneNumberValidation = RegexValidations.IsValidPhoneNumber(writerDetail.PhoneNumber);
                var nameValidation = RegexValidations.IsValidName(writerDetail.FirstName, writerDetail.LastName, writerDetail.MiddleName ?? "");
                var passwordValidation = RegexValidations.IsAcceptablePasswordFormat(writerDetail.Password);
                var validationErrors = new List<string>();

                if (!emailValidation) validationErrors.Add("Invalid email");
                if (!phoneNumberValidation) validationErrors.Add("Invalid phone number");
                if (!nameValidation) validationErrors.Add("Names can only contain alphabets");
                if (!passwordValidation) validationErrors.Add("Password must be strong");

                if (validationErrors.Count > 0)
                {
                    return ResponseDetail<GetWriterDetailDTO>.Failed(string.Join(" | ", validationErrors));
                }

                var writerAccount = await dbContext.AuthProfiles.Select(x => new { x.Email, x.IsDeleted }).FirstOrDefaultAsync(w => w.Email == email);

                if (writerAccount?.IsDeleted == true)
                {
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"Profile with email {writerDetail.Email} already exists and just needs to be reactivated", 409, "Account needs to be reactivated");
                }
                if (writerAccount is not null)
                {
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"Profile with email {writerDetail.Email} already exists", 409, "Conflict");
                }
                var writer = new Writer()
                {
                    FirstName = writerDetail.FirstName.ToUpperInvariant(),
                    LastName = writerDetail.LastName.ToUpperInvariant(),
                    Email = email,
                    PhoneNumber = writerDetail.PhoneNumber,
                    Bio = writerDetail.Bio,
                    DateOfBirth = writerDetail.DateOfBirth,
                    Address = new Address
                    {
                        City = writerDetail.AddressDetail.City.ToUpperInvariant(),
                        State = writerDetail.AddressDetail.State.ToUpperInvariant(),
                        Country = writerDetail.AddressDetail.Country.ToUpperInvariant(),
                        PostalCode = writerDetail.AddressDetail.PostalCode.ToUpperInvariant(),
                        Street = writerDetail.AddressDetail.Street.ToUpperInvariant(),
                        AdditionalDetails = writerDetail.AddressDetail.AdditionalDetails
                    },
                    Gender = writerDetail.Gender,
                    IsPremiumMember = writerDetail.IsPremiumMember,
                    MiddleName = writerDetail.MiddleName?.ToUpperInvariant() ?? "",
                    Type = Role.Writer,
                };

                AuthProfile newAuthProfile = new()
                {
                    Email = email,
                    Password = BCrypt.Net.BCrypt.HashPassword(writerDetail.Password),
                    Role = "Writer",
                    FullName = $"{writerDetail.FirstName} {writerDetail.LastName}".ToUpperInvariant(),
                    UserId = writer.Id,
                };
                var services = writerDetail.PostServiceDetail?
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
                                    WriterId = writer.Id
                                })
                                .ToList() ?? [];

                Wallet wallet = new Wallet
                {
                    TotalBalance = 0,
                    AvailableBalance = 0,
                    LockedBalance = 0,
                    Currency = Currency.NAIRA,
                    UserId = writer.Id,
                };

                writer.AuthProfile = newAuthProfile;
                writer.Services = services;
                writer.Wallet = wallet;

                var userDirectoryName = $"Writer_{writer.FirstName}_{writer.LastName}-{writer.PhoneNumber}";
                var documentId = await fileService.ProcessDocumentForUpload(userDirectoryName, writerDetail.VerificationDocument);

                if (documentId.Data == Guid.Empty || documentId == null)
                {
                    logger.LogError($"Error uploading KYC document {writerDetail.FirstName} {writerDetail.LastName} ");
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"An error occurred while uploading the KYC document for: {writerDetail.FirstName} {writerDetail.LastName}", 500, "UnexpectedError");
                }
                writer.DocumentID = documentId.Data;

                await dbContext.Writers.AddAsync(writer);
                var writerRes = await dbContext.SaveChangesAsync();

                if (writerRes < 1)
                {
                    logger.LogError($"Error saving writer profile {writer.FirstName} {writer.LastName} to the database");
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"Error saving writer profile {writer.FirstName} {writer.LastName} to the database");
                }

                var token = RandomNumberGenerator.GetInt32(100000, 999999);

                cache.Set($"User_Verification_Token_{writer.Id}", token, absoluteExpiration: DateTimeOffset.UtcNow.AddMinutes(10));
                Console.WriteLine($"Writer_Verification_Token_{writer.FirstName} {writer.LastName}", token);

                var verificationMail = MailNotifications.RegistrationConfirmationMailNotification(writer.Email, writer.FirstName, token.ToString());
                BackgroundJob.Enqueue(() => hangfire.SendMailAsync(verificationMail));

                var kycDetail = new YouVerifyKycDto
                {
                    Id = writerDetail.VerificationDocument.VerificationNumber,
                    Type = writerDetail.VerificationDocument.Type.ToString(),
                    UserId = writer.Id,
                    LastName = writer.LastName,
                };
                BackgroundJob.Enqueue(() => hangfire.StartKycProcess(kycDetail));

                await transaction.CommitAsync();

                var writerProfile = new GetWriterDetailDTO
                {
                    Id = writer.Id,
                    FirstName = writer.FirstName,
                    LastName = writer.LastName,
                    MiddleName = writer.MiddleName,
                    Name = $"{writer.FirstName} {writer.LastName} {writer.MiddleName}",
                    Bio = writer.Bio,
                    Email = writer.Email,
                    PhoneNumber = writer.PhoneNumber,
                    IsBlacklisted = writer.IsDeleted,
                    IsEmailVerified = writer.AuthProfile.IsEmailVerified,
                    IsVerified = writer.AuthProfile.IsVerified,
                    Role = writer.AuthProfile.Role,
                    IsPremium = writer.IsPremiumMember,
                    VerificationStatus = writer.VerificationStatus.ToString(),
                    Address = new AddressDetail
                    {
                        City = writer.Address.City,
                        State = writer.Address.State,
                        Country = writer.Address.Country,
                        PostalCode = writer.Address.PostalCode,
                        Street = writer.Address.Street,
                        AdditionalDetails = writer.Address.AdditionalDetails ?? ""
                    },
                    Services = writer.Services.Select(s => new GetServiceDetailDTO
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
                        Id = writer.Wallet.Id,
                        Balance = writer.Wallet.TotalBalance,
                        LockedBalance = writer.Wallet.LockedBalance,
                        Currency = writer.Wallet.Currency,
                        CurrencySymbol = writer.Wallet.CurrencySymbol,
                        UserId = writer.Id
                    },
                    CreatedAt = writer.CreatedAt,
                    DateCreated = writer.DateCreated,
                    TimeCreated = writer.TimeCreated,
                    DateModified = writer.DateModified,
                    ModifiedAt = writer.ModifiedAt,
                    TimeModified = writer.TimeModified
                };

                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(3),
                };
                cache.Set($"Writer_Profile{writer.Id}", writerProfile, options);
                return ResponseDetail<GetWriterDetailDTO>.Successful(writerProfile, "Writer profile created successfully", 201);
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                logger.LogError($"A database update exception was thrown while creating a writer profile: {dbEx.GetType().Name}", dbEx.Message);
                return ResponseDetail<GetWriterDetailDTO>.Failed("An Db update exception was thrown while saving writer profile", dbEx.HResult, "Database Update Error");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError($"An exception {ex.GetType().FullName} was thrown while creating a writer profile...\n Base Exception: {ex.GetBaseException().GetType().FullName}", ex.Message);
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