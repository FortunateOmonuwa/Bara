using Services.FileStorageServices.Interfaces;

namespace Services.FileStorageServices.Repository
{
    public class FileStorageService : IFileStorageService
    {
        public Task<bool> DeleteAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public Task<FileDetailResponse> UploadAsync(Stream fileStream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
