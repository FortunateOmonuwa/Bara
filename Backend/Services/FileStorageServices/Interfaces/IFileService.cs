using Microsoft.AspNetCore.Http;
using SharedModule.Utils;

namespace Services.FileStorageServices.Interfaces
{
    public interface IFileService
    {
        Task<ResponseDetail<FileDetailResponse>> ProcessDocumentForUpload(IFormFile file);
    }
}
