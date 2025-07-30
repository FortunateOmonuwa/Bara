using Microsoft.AspNetCore.Http;

namespace Services.FileStorageServices.Interfaces
{
    public interface IFileStorageService
    {
        Task<bool> UploadDocumentAsync(string userDirectoryName, IFormFile file);
        Task<bool> UploadScriptAsync(string userDirectoryName, IFormFile file);
        Task<Stream> DownloadAsync(Guid fileId);
        Task<bool> DeleteAsync(Guid fileId);
    }
}
