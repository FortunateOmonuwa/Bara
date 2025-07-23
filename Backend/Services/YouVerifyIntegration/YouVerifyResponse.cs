namespace Services.YouVerifyIntegration
{
    /// <summary>
    /// Represents the response structure from YouVerify after a verification request.
    /// </summary>
    public class YouVerifyResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ReponseData Data { get; set; }
    }

    /// <summary>
    /// Represents the data returned in the YouVerify response after a verification request.
    /// </summary>
    public class ReponseData
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string DateOfBirth { get; set; }
    }
}
