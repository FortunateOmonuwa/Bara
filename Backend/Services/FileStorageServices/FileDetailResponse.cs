namespace Services.FileStorageServices
{
    public record FileDetailResponse
    {
        public string Name { get; init; }
        public string Path { get; init; }
        public string Url { get; init; }
        public string FilExtension { get; init; }
    }

    public class FolderResponse
    {
        public List<Folder> Folders { get; set; }
        public string? NextCursor { get; set; }
        public int TotalCount { get; set; }
    }

    public class Folder
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ExternalId { get; set; }
    }

}
