namespace Services.YouVerifyIntegration
{
    /// <summary>
    /// Represents the KYC data required for a YouVerify verification request.
    /// </summary>
    public record YouVerifyKycDto
    {
        /// <summary>
        /// The unique identifier for the subject (e.g., BVN, NIN, etc.).
        /// </summary>
        public string Id { get; init; } = default!;

        /// <summary>
        /// The document type for verification (e.g., "bvn", "nin", "passport").
        /// </summary>
        public string Type { get; init; } = default!;

        /// <summary>
        /// The last name of the subject for additional identity matching.
        /// </summary>
        public string LastName { get; init; } = default!;

        /// <summary>
        /// Indicates whether the subject has consented to the verification.
        /// </summary>
        public bool IsSubjectConsent { get; init; } = true;
    }
}
