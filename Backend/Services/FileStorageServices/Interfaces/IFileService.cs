using SharedModule.Utils;
using UserModule.DTOs.DocumentDTOs;

namespace Services.FileStorageServices.Interfaces
{
    public interface IFileService
    {
        Task<ResponseDetail<Guid>> ProcessDocumentForUpload(string userDirectoryName, PostDocumentDetailDTO documentDetail);
    }
}
