using Hangfire;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Services.BackgroudServices;
using Services.FileStorageServices.Interfaces;
using Services.MailingService;
using Shared.Models;
using SharedModule.DTOs.AddressDTOs;
using SharedModule.Utils;
using System.Security.Cryptography;
using TransactionModule.Models;
using UserModule.DTOs.ProducerDTOs;
using UserModule.Interfaces.UserInterfaces;
using UserModule.Models;

namespace Infrastructure.Repositories.UserRepositories
{
    public class ProducerRepository : IProducerService
    {
        private readonly BaraContext dbContext;
        private readonly ILogger<WriterRepository> logger;
        private readonly IFileService fileService;
        private readonly HangfireJobs hangfire;
        private readonly IMemoryCache cache;
        public ProducerRepository(BaraContext baraContext, ILogger<WriterRepository> logger, IFileService fileService, HangfireJobs hangfireJobs, IMemoryCache memoryCache)
        {
            dbContext = baraContext;
            this.logger = logger;
            this.fileService = fileService;
            hangfire = hangfireJobs;
            cache = memoryCache;
        }
        public async Task<ResponseDetail<GetProducerDetailDTO>> AddProducer(PostProducerDetailDTO producerDetailDTO)
        {
            var transaction = dbContext.Database.BeginTransaction();
            try
            {
                var producerAccount = await dbContext.AuthProfiles
                                                            .Select(x => new { x.Email, x.IsDeleted })
                                                            .FirstOrDefaultAsync(x => x.Email == producerDetailDTO.Email);
                if (producerAccount?.IsDeleted == true)
                {
                    return ResponseDetail<GetProducerDetailDTO>.Failed("A profile with this email already exists and just needs to be Reactivated", 409, "Account Needs Reactivation");
                }
                else if (producerAccount is not null)
                {
                    return ResponseDetail<GetProducerDetailDTO>.Failed("A profile with this email already exists...", 409, "conflict");
                }
                var newProducerProfile = new Producer
                {
                    Email = producerDetailDTO.Email,
                    FirstName = producerDetailDTO.FirstName,
                    LastName = producerDetailDTO.LastName,
                    MiddleName = producerDetailDTO.MiddleName,
                    PhoneNumber = producerDetailDTO.PhoneNumber,
                    Bio = producerDetailDTO.Bio,
                    Gender = producerDetailDTO.Gender,
                    DateOfBirth = producerDetailDTO.DateOfBirth,
                    Role = "Producer",
                    Address = new Address
                    {
                        City = producerDetailDTO.AddressDetail.City,
                        Country = producerDetailDTO.AddressDetail.Country,
                        State = producerDetailDTO.AddressDetail.State,
                        Street = producerDetailDTO.AddressDetail.Street,
                        PostalCode = producerDetailDTO.AddressDetail.PostalCode,
                        AdditionalDetails = producerDetailDTO.AddressDetail.AdditionalDetails,
                    },
                    AuthProfile = new AuthProfile
                    {
                        Email = producerDetailDTO.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(producerDetailDTO.Password),
                        Role = "Producer",
                    },
                    Wallet = new Wallet
                    {
                        Balance = 0,
                    },
                };

                var userDirectoryName = $"Producer_{newProducerProfile.FirstName}_{newProducerProfile.LastName}-{newProducerProfile.PhoneNumber}";
                var documentId = await fileService.ProcessDocumentForUpload(userDirectoryName, producerDetailDTO.VerificationDocument);
                if (documentId.Data.ToString() == null)
                {
                    logger.LogError($"An error occured while uploading KYC document for {producerDetailDTO.FirstName} {producerDetailDTO.LastName}");
                    return ResponseDetail<GetProducerDetailDTO>.Failed($"An error occured while uploading KYC document for {producerDetailDTO.FirstName} {producerDetailDTO.LastName}", 500, "Unexpected Error");
                }
                newProducerProfile.VerificationDocumentID = documentId.Data;

                await dbContext.Producers.AddAsync(newProducerProfile);
                var producerRes = await dbContext.SaveChangesAsync();
                if (producerRes < 1)
                {
                    logger.LogError($"An error occured while creating a producer profile for {producerDetailDTO.FirstName} {producerDetailDTO.LastName}");
                    return ResponseDetail<GetProducerDetailDTO>.Failed($"An error occured while creating a producer profile for {producerDetailDTO.FirstName} {producerDetailDTO.LastName}", 500, "Unexpected Error");
                }

                var token = RandomNumberGenerator.GetInt32(100000, 999999);
                cache.Set($"Producer_Verification_Token_{newProducerProfile.Id}", token, absoluteExpiration: DateTimeOffset.UtcNow.AddMinutes(10));

                var verificationMail = MailNotifications.RegistrationConfirmationMailNotification(newProducerProfile.Email, newProducerProfile.FirstName, token.ToString());
                BackgroundJob.Enqueue<HangfireJobs>(x => x.SendMailAsync(verificationMail));

                await transaction.CommitAsync();

                var producerProfile = new GetProducerDetailDTO
                {
                    Id = newProducerProfile.Id,
                    Email = newProducerProfile.Email,
                    FirstName = newProducerProfile.FirstName,
                    LastName = newProducerProfile.LastName,
                    MiddleName = newProducerProfile.MiddleName,
                    Name = $"{newProducerProfile.FirstName} {newProducerProfile.LastName}",
                    Bio = newProducerProfile.Bio,
                    IsBlacklisted = newProducerProfile.IsBlacklisted,
                    Address = new AddressDetail
                    {
                        City = newProducerProfile.Address.City,
                        Country = newProducerProfile.Address.Country,
                        State = newProducerProfile.Address.State,
                        Street = newProducerProfile.Address.Street,
                        PostalCode = newProducerProfile.Address.PostalCode,
                        AdditionalDetails = newProducerProfile.Address.AdditionalDetails,
                    },
                    IsDeleted = newProducerProfile.IsDeleted,
                    IsEmailVerified = newProducerProfile.IsEmailVerified,
                    IsVerified = newProducerProfile.IsVerified,
                    PhoneNumber = newProducerProfile.PhoneNumber,
                    VerificationStatus = newProducerProfile.VerificationStatus
                };

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                cache.Set($"Producer_Profile{newProducerProfile.Id}", producerProfile, cacheOptions);
                return ResponseDetail<GetProducerDetailDTO>.Successful(producerProfile, "Producer profile created successfully", 201);
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                logger.LogError($"A database update exception was thrown while creating an account for {producerDetailDTO.FirstName} {producerDetailDTO.LastName}: {dbEx.InnerException}", dbEx.Message);
                return ResponseDetail<GetProducerDetailDTO>.Failed(dbEx.InnerException?.Message ?? "Database update error", dbEx.HResult, "Database Update Error");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError($"An exception {ex.InnerException} was thrown while creating a writer profile", ex.Message);
                return ResponseDetail<GetProducerDetailDTO>.Failed(ex.Message, ex.HResult, "Caught Exception");
            }
        }

        public Task<bool> DeleteProducer(Guid producerId)
        {
            //set as is deleted and delete auth profile 
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<GetProducerDetailDTO>> GetProducer(Guid producerId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<GetProducerDetailDTO>> UpdateProducer(PostProducerDetailDTO producerDetailDTO, Guid producerId)
        {
            throw new NotImplementedException();
        }
    }
}
