namespace Services.FileStorageServices
{
    public record FileDetailResponse
    {
        public string Name { get; init; }
        public string Path { get; init; }
        public string Url { get; init; }
        public string FilExtension { get; init; }
    }
}
