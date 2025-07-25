using Infrastructure.DataContext;
using SharedModule.Utils;
using UserModule.DTOs.WriterDTOs;
using UserModule.Interfaces.UserInterfaces;

namespace Infrastructure.Repositories.UserRepositories
{
    public class WriterRepository : IWriterService
    {
        private readonly BaraContext dbContext;
        public WriterRepository(BaraContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task<ResponseDetail<GetWriterDetailDTO>> AddWriter(PostWriterDetailDTO writerDetail)
        {
            throw new NotImplementedException();
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
