namespace Services.YouVerifyIntegration
{
    public record YouVerify_KYC_DTO
    (
        string Id,
        string Type,
        string LastName,
        bool IsSubjectConsent = true

    );
}
