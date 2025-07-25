namespace Services.FileStorageServices
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName);
        Task<Stream> DownloadAsync(Guid fileId);
        Task<bool> DeleteAsync(Guid fileId);
    }
}
