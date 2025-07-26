using Infrastructure.DataContext;
using Microsoft.AspNetCore.Http;
using Services.FileStorageServices.Interfaces;
using SharedModule.Utils;

namespace Services.FileStorageServices.Repository
{
    /// <summary>
    /// Service class for handling file operations and saving them to the database
    /// </summary>
    public class FileRepository : IFileService
    {
        private readonly IFileStorageService storageService;
        private readonly BaraContext context;
        public FileRepository(IFileStorageService fileStorageService, BaraContext baraContext)
        {
            storageService = fileStorageService;
            context = baraContext;
        }

        public async Task<ResponseDetail<FileDetailResponse>> ProcessDocumentForUpload(IFormFile file)
        {
            try
            {
                long limit = 2 * 1024 * 1024;
                var fileSizeExceedLimit = file.Length > limit;
                if (fileSizeExceedLimit)
                {
                    return ResponseDetail<FileDetailResponse>.Failed($"Document exceeds limit of {limit}", 413, "File Limit Exceeded");
                }

                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                var allowedMimeTypes = new[] { "application/pdf", "image/jpeg", "image/png" };

                var fileName = file.FileName.Trim();
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                var fileFormatIsAcceptable = allowedExtensions.Contains(fileExtension) &&
                                             allowedMimeTypes.Contains(file.ContentType);
                if (!fileFormatIsAcceptable)
                {
                    return ResponseDetail<FileDetailResponse>.Failed($"File format {fileExtension} or mime type {file.ContentType} is not supported. " +
                                                                    $"Allowed Extensions are: {string.Join(", ", allowedExtensions)}..." +
                                                                    $"\n Allowed mime types are: {string.Join(", ", allowedMimeTypes)}", 415, "Invalid File Type");
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
