using Newtonsoft.Json;

namespace Services.YouVerifyIntegration
{
    public record YouVerify_KYC_DTO
    (
        [property: JsonProperty("id")]
        string Id,
        [property: JsonProperty("isSubjectConsent")]
        bool IsSubjectConsent,
        [property: JsonProperty("type")]
        string Type
    //[property: JsonProperty("lastName")]
    //string LastName = ""
    );

    public record YouVerify_KYC_DTO_PassPort
    (
        [property: JsonProperty("id")]
        string Id,
        [property: JsonProperty("isSubjectConsent")]
        bool IsSubjectConsent = true
    //[property: JsonProperty("lastName")]
    //string LastName = ""
    );
}
