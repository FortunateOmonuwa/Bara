using Infrastructure.DataContext;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.FileStorageServices.Interfaces;
using Shared.Models;
using SharedModule.DTOs.AddressDTOs;
using SharedModule.Settings;
using SharedModule.Utils;
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
        public WriterRepository(BaraContext dbContext, ILogger<WriterRepository> logger, IOptions<AppSettings> appSettings, IFileService fileService)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            settings = appSettings.Value;
            this.fileService = fileService;
        }
        public async Task<ResponseDetail<GetWriterDetailDTO>> AddWriter(PostWriterDetailDTO writerDetail)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
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
                    },
                    Wallet = new Wallet
                    {
                        Balance = 0,
                    },
                };

                await dbContext.Writers.AddAsync(writer);
                var writerRes = await dbContext.SaveChangesAsync();

                if (writerRes < 1)
                {
                    logger.LogError($"Error saving writer profile {writer.FirstName} {writer.LastName} to the database");
                    await transaction.RollbackAsync();
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"Error saving writer profile {writer.FirstName} {writer.LastName} to the database");
                }
                var userDirectoryName = $"{writer.FirstName}_{writer.LastName}-{writer.Id}";
                var uploadDocsOp = await fileService.ProcessDocumentForUpload(userDirectoryName, writerDetail.VerificationDocument);

                if (uploadDocsOp.Data.ToString() == null)
                {
                    logger.LogError($"Error uploading verification document: for writer {writer.Id} ");
                    await transaction.RollbackAsync();
                    return ResponseDetail<GetWriterDetailDTO>.Failed("Error uploading verification document: for writer {writer.Id}");
                }

                //Verify the document and setup a background service to handle this
                writer.VerificationDocumentID = uploadDocsOp.Data;
                dbContext.Update(writer);

                var updateWriterRes = await dbContext.SaveChangesAsync();
                if (updateWriterRes < 1)
                {
                    logger.LogError($"Error updating writer profile {writer.FirstName} {writer.LastName} with verification document");
                    await transaction.RollbackAsync();
                    return ResponseDetail<GetWriterDetailDTO>.Failed($"Error updating writer profile {writer.FirstName} {writer.LastName} with verification document");
                }

                //Send verification mail before commiting transaction

                await transaction.CommitAsync();

                var finalResponse = new GetWriterDetailDTO
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
                        Genre = s.Genre
                    }).ToList(),
                    Wallet = new GetWalletDetailDTO
                    {
                        Id = writer.Wallet.Id,
                        Balance = writer.Wallet.Balance,
                        Currency = writer.Wallet.Currency,
                        CurrencySymbol = writer.Wallet.CurrencySymbol
                    },
                    CreatedAt = writer.CreatedAt,
                    DateCreated = writer.DateCreated,
                    TimeCreated = writer.TimeCreated,
                    DateModified = writer.DateModified,
                    ModifiedAt = writer.ModifiedAt,
                    TimeModified = writer.TimeModified
                };
                return ResponseDetail<GetWriterDetailDTO>.Successful(finalResponse, "Writer profile created successfully", 201);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, $"An exception {ex.InnerException} was thrown while creating a writer profile");
                return ResponseDetail<GetWriterDetailDTO>.Failed(ex.Message, ex.HResult, "Caught Exception");
            }
        }

        public Task<ResponseDetail<bool>> DeleteWriter(Guid writerId)
        {
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
