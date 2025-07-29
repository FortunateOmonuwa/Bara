namespace Services.YouVerifyIntegration
{
    public record YouVerify_KYC_DTO
    {
        public string Id { get; init; } = default!;
        public string Type { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public bool IsSubjectConsent { get; init; } = true;
    }
}
