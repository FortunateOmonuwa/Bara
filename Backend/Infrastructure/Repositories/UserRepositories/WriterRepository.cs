using Infrastructure.DataContext;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Models;
using SharedModule.Settings;
using SharedModule.Utils;
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
        public WriterRepository(BaraContext dbContext, ILogger<WriterRepository> logger, IOptions<AppSettings> appSettings)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            settings = appSettings.Value;
        }
        public async Task<ResponseDetail<GetWriterDetailDTO>> AddWriter(PostWriterDetailDTO writerDetail)
        {
            var transaction = await dbContext.Database.BeginTransactionAsync();
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
                    VerificationDocument = new Document
                    {
                        DocumentType = writerDetail.VerificationDocument.Type,
                        IdentificationNumber = writerDetail.VerificationDocument.VerificationNumber,

                    },


                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding writer");
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
