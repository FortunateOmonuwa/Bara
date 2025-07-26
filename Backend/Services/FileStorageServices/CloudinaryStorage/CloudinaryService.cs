using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Services.FileStorageServices.Interfaces;
using SharedModule.Settings;

namespace Services.FileStorageServices.CloudinaryStorage
{
    public class CloudinaryService : IFileStorageService
    {
        private readonly string CloudinaryBaseURL = "";
        const string CREATE_FOLDER_URL = "/folders/:folder";
        private readonly AppSettings settings;
        private readonly Secrets secrets;
        public CloudinaryService(Secrets secrets, IOptions<AppSettings> appSettings)
        {
            this.secrets = secrets;
            settings = appSettings.Value;
            CloudinaryBaseURL = $"{settings.CloudinaryBaseURL}/{secrets.CloudinaryName}";
        }
        public Task<bool> DeleteAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UploadDocumentAsync(Stream fileStream, string fileName)
        {
            try
            {
                var cloudinary = new Cloudinary
                {

                };
            }
            catch
            {
                return false;
            }
        }
    }
}
