namespace Services.YouVerifyIntegration
{
    public record YouVerify_KYC_DTO
    (
        string ID,
        bool IsSubjectConsent,
        string Type,
        string LastName = ""
    );
}
