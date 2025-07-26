namespace Services.FileStorageServices.Interfaces
{
    public interface IFileStorageService
    {
        Task<bool> UploadDocumentAsync(Stream fileStream, string fileName);
        Task<Stream> DownloadAsync(Guid fileId);
        Task<bool> DeleteAsync(Guid fileId);
    }
}
