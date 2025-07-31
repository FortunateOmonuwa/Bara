using Hangfire;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.BackgroudServices;
using Services.FileStorageServices.Interfaces;
using Services.MailingService;
using Shared.Models;
using SharedModule.DTOs.AddressDTOs;
using SharedModule.Settings;
using SharedModule.Utils;
using System.Security.Cryptography;
using TransactionModule.DTOs;
using TransactionModule.Models;
using UserModule.DTOs.ServiceDTOs;
using UserModule.DTOs.WriterDTOs;
using UserModule.Interfaces.UserInterfaces;
using UserModule.Models;

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
                var writerAccount = await dbContext.AuthProfiles.Select(x => new { x.Email, x.IsDeleted }).FirstOrDefaultAsync(w => w.Email == writerDetail.Email);

                if (writerAccount?.IsDeleted == true)
                {
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"Profile with email {writerDetail.Email} already exists and just needs to be reactivated", 409, "Account needs to be reactivated");
                }
                if (writerAccount is not null)
                {
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"Profile with email {writerDetail.Email} already exists", 409, "Conflict");
                }
                var writer = new Writer
                {
                    FirstName = writerDetail.FirstName,
                    LastName = writerDetail.LastName,
                    Email = writerDetail.Email,
                    PhoneNumber = writerDetail.PhoneNumber,
                    Bio = writerDetail.Bio,
                    DateOfBirth = writerDetail.DateOfBirth,
                    Address = new Address
                    {
                        City = writerDetail.AddressDetail.City,
                        State = writerDetail.AddressDetail.State,
                        Country = writerDetail.AddressDetail.Country,
                        PostalCode = writerDetail.AddressDetail.PostalCode,
                        Street = writerDetail.AddressDetail.Street,
                        AdditionalDetails = writerDetail.AddressDetail.AdditionalDetails
                    },
                    Gender = writerDetail.Gender,
                    IsPremiumMember = writerDetail.IsPremiumMember,
                    MiddleName = writerDetail.MiddleName,
                    Role = "Writer",
                    Services = writerDetail.PostServiceDetail?
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
                                    Genre = dto.Genre ?? []
                                })
                                .ToList() ?? [],
                    AuthProfile = new AuthProfile
                    {
                        Email = writerDetail.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(writerDetail.Password),
                        Role = "Writer",
                    },
                    Wallet = new Wallet
                    {
                        Balance = 0,
                    },
                };

                var userDirectoryName = $"Writer_{writer.FirstName}_{writer.LastName}-{writer.PhoneNumber}";
                var documentId = await fileService.ProcessDocumentForUpload(userDirectoryName, writerDetail.VerificationDocument);

                if (documentId.Data == Guid.Empty)
                {
                    logger.LogError($"Error uploading KYC document {writerDetail.FirstName} {writerDetail.LastName} ");
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"An error occurred while uploading the KYC document for: {writerDetail.FirstName} {writerDetail.LastName}", 500, "UnexpectedError");
                }
                writer.VerificationDocumentID = documentId.Data;

                await dbContext.Writers.AddAsync(writer);
                var writerRes = await dbContext.SaveChangesAsync();

                if (writerRes < 1)
                {
                    logger.LogError($"Error saving writer profile {writer.FirstName} {writer.LastName} to the database");
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"Error saving writer profile {writer.FirstName} {writer.LastName} to the database");
                }

                var token = RandomNumberGenerator.GetInt32(100000, 999999);

                cache.Set($"Writer_Verification_Token_{writer.Id}", token, absoluteExpiration: DateTimeOffset.UtcNow.AddMinutes(10));

                var verificationMail = MailNotifications.RegistrationConfirmationMailNotification(writer.Email, writer.FirstName, token.ToString());
                BackgroundJob.Enqueue(() => hangfire.SendMailAsync(verificationMail));

                await transaction.CommitAsync();

                var writerProfile = new GetWriterDetailDTO
                {
                    Id = writer.Id,
                    FirstName = writer.FirstName,
                    LastName = writer.LastName,
                    MiddleName = writer.MiddleName ?? "",
                    Name = $"{writer.FirstName} {writer.LastName} {writer.MiddleName ?? ""}",
                    Bio = writer.Bio,
                    Email = writer.Email,
                    PhoneNumber = writer.PhoneNumber,
                    IsBlacklisted = writer.IsBlacklisted,
                    IsDeleted = writer.IsDeleted,
                    IsEmailVerified = writer.IsEmailVerified,
                    IsVerified = writer.IsVerified,
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
                        Balance = writer.Wallet.Balance,
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
                logger.LogError($"A database update exception was thrown while creating a writer profile: {dbEx.InnerException}", dbEx.Message);
                return ResponseDetail<GetWriterDetailDTO>.Failed(dbEx.InnerException?.Message ?? "Database update error", dbEx.HResult, "Database Update Error");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError($"An exception {ex.InnerException} was thrown while creating a writer profile", ex.Message);
                return ResponseDetail<GetWriterDetailDTO>.Failed(ex.Message, ex.HResult, "Caught Exception");
            }
        }

        public Task<ResponseDetail<bool>> DeleteWriter(Guid writerId)
        {
            //Set as deleted and delete login profile
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<GetWriterDetailDTO>> GetWriterDetail(Guid writerId)
        {
            throw new NotImplementedException();
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
