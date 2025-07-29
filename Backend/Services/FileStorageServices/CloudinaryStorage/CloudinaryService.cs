using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.FileStorageServices.Interfaces;
using SharedModule.Settings;
using System.Net;

namespace Services.FileStorageServices.CloudinaryStorage
{
    public class CloudinaryService : IFileStorageService
    {
        private readonly string CloudinaryBaseURL = "";
        const string FOLDER_URL = "/folders/:folder";
        const string Search_FOLDER_URL = "/folders/search";

        private readonly AppSettings settings;
        private readonly Secrets secrets;
        private readonly ILogger<CloudinaryService> logger;
        private readonly string BaseFolder = "";
        public CloudinaryService(IOptions<Secrets> secrets, IOptions<AppSettings> appSettings, ILogger<CloudinaryService> logger)
        {
            this.secrets = secrets.Value;
            settings = appSettings.Value;
            CloudinaryBaseURL = $"{settings.CloudinaryBaseURL}/{secrets.Value.CloudinaryName}";
            this.logger = logger;
            BaseFolder = secrets.Value.CloudinaryFolderName;
        }
        public Task<bool> DeleteAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UploadDocumentAsync(string userDirectoryName, IFormFile file)
        {
            try
            {
                var cloudinary = new Cloudinary(new Account(secrets.CloudinaryName, secrets.CloudinaryAPIKEY, secrets.CloudinaryAPISecret));

                var documentFolder = $"{BaseFolder}/{userDirectoryName}/documents";

                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = documentFolder
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error uploading document to Cloudinary for {userDirectoryName}", ex.Message);
                return false;
            }
        }
        public async Task<bool> UploadScriptAsync(string userDirectoryName, IFormFile file)
        {
            try
            {
                var cloudinary = new Cloudinary(new Account(secrets.CloudinaryName, secrets.CloudinaryAPIKEY, secrets.CloudinaryAPISecret));

                var scriptFolder = $"{BaseFolder}/{userDirectoryName}/scripts";

                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = scriptFolder
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);
                return uploadResult?.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error uploading script titled {file.Name} to Cloudinary for {userDirectoryName}", ex.Message);
                return false;
            }
        }
        private async Task<FolderResponse> GetAllFolders()
        {
            try
            {
                var cloudinary = new Cloudinary
                {

                };
                var folders = await cloudinary.SubFoldersAsync(secrets.CloudinaryFolderName);
                return new FolderResponse
                {
                    Folders = folders.Folders.Select(f => new Folder
                    {
                        Name = f.Name,
                        Path = f.Path,
                    }).ToList(),
                    NextCursor = null,
                    TotalCount = folders.TotalCount
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching folders: {ex.Message}");
            }
        }
    }
}
