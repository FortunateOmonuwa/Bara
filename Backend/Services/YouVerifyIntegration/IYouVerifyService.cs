namespace Services.YouVerifyIntegration
{
    /// <summary>
    /// Represents the contracts used to verify a user document on YouVerify
    /// </summary>
    public interface IYouVerifyService
    {
        Task<YouVerifyResponse> VerifyDocumentAsync(YouVerify_KYC_DTO details);
        //Task<ResponseDetail<YouVerifyResponse>> VerifyInternationalPassAsync(YouVerify_KYC_DTO details);
    }
}
