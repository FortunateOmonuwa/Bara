using Newtonsoft.Json;

namespace Services.YouVerifyIntegration
{
    /// <summary>
    /// Represents the response structure from YouVerify after a verification request.
    /// </summary>
    public class YouVerifyResponse
    {
        [property: JsonProperty("success")]
        public bool Success { get; set; }
        [property: JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        [property: JsonProperty("message")]
        public string Message { get; set; }
        [property: JsonProperty("data")]
        public ReponseData Data { get; set; }
    }

    public class YouVerifyErrorResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Represents the data returned in the YouVerify response after a verification request.
    /// </summary>
    public class ReponseData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string DateOfBirth { get; set; }
    }
}
