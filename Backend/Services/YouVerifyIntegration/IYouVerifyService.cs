namespace Services.YouVerifyIntegration
{
    /// <summary>
    /// Represents the contract for verifying user documents via YouVerify.
    /// </summary>
    public interface IYouVerifyService
    {
        /// <summary>
        /// Sends a verification request to YouVerify using provided user details.
        /// </summary>
        /// <param name="details">The user details and document information required for verification.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="YouVerifyKickoffResponse"/> containing the verification outcome.</returns>
        Task<YouVerifyKickoffResponse> VerifyIdentificationNumberAsync(YouVerifyKycDto details, CancellationToken cancellationToken);

        // Task<ResponseDetail<YouVerifyResponse>> VerifyInternationalPassAsync(YouVerifyKycDto details);
    }

}
