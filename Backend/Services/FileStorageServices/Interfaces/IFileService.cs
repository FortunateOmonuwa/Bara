using SharedModule.Utils;
using UserModule.DTOs.DocumentDTOs;

namespace Services.FileStorageServices.Interfaces
{
    /// <summary>
    /// Defines the contract for processing and handling document uploads for users.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Processes and uploads a user's verification document to storage.
        /// </summary>
        /// <param name="userDirectoryName">The base directory or folder name assigned to the user.</param>
        /// <param name="documentDetail">The document details including file, metadata, and type.</param>
        /// <returns>A <see cref="ResponseDetail{Guid}"/> containing the document's unique identifier or error information.</returns>
        Task<ResponseDetail<Guid>> ProcessDocumentForUpload(string userDirectoryName, PostDocumentDetailDTO documentDetail);
    }
}
