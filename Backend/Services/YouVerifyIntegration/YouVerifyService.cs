using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.ExternalAPI_Integration;
using SharedModule.Settings;

namespace Services.YouVerifyIntegration
{
    public class YouVerifyService : IYouVerifyService
    {
        private readonly ExternalApiIntegrationService apiService;
        private readonly Secrets secrets;
        private readonly AppSettings settings;

        /// <summary>
        /// Urls
        /// </summary>
        private readonly string BaseUrl;
        private readonly string BVN_URL;
        private readonly string Passport_URL;
        private readonly string NIN_URL;
        private readonly string Drivers_License_URL;

        public YouVerifyService(ExternalApiIntegrationService apiService, IOptions<Secrets> appSecrets, IOptions<AppSettings> appSettings)
        {
            this.apiService = apiService;
            secrets = appSecrets.Value;
            settings = appSettings.Value;
            BaseUrl = settings.YouVerifyBaseUrl;
            BVN_URL = $"{settings.YouVerify_BVN_VerificationUrl}";
            Passport_URL = $"{settings.YouVerify_International_Passport_VerificationUrl}";
            NIN_URL = $"{settings.YouVerify_NIN_VerificationUrl}";
            Drivers_License_URL = $"{settings.YouVerify_Drivers_License_VerificationUrl}";
        }

        public async Task<YouVerifyResponse> VerifyDocumentAsync(YouVerify_KYC_DTO details)
        {
            var url = details.Type switch
            {
                "BVN" => BVN_URL,
                "NIN" => NIN_URL,
                "International_Passport" => Passport_URL,
                "Drivers_License" => Drivers_License_URL,
                _ => throw new ArgumentException($"Unsupported document type: {details.Type}")
            };
            object body = details.Type switch
            {
                "BVN" or "NIN" or "Drivers_License" => new
                {
                    id = details.Id,
                    isSubjectConsent = true,
                    premiumBVN = false,
                    metadata = new { requestId = Guid.NewGuid().ToString() }
                },
                "International_Passport" => new
                {
                    id = details.Id,
                    isSubjectConsent = true,
                    lastName = details.LastName,
                    premiumBVN = false,
                    metadata = new { requestId = Guid.NewGuid().ToString() }
                },
                _ => throw new ArgumentException($"Unsupported document type: {details.Type}")
            };

            var reqBody = apiService.SerializeReqBody(body);
            //var header = new Dictionary<string, string>
            //{
            //    { "token", secrets.YouVerifyLiveAPIKEY },
            //};
            var request = await apiService.SendPostRequest(reqBody, url, null, "YouVerify");
            if (!request.IsSuccessStatusCode)
            {
                var errorResponse = await request.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<YouVerifyErrorResponse>(errorResponse)
                    ?? throw new HttpRequestException("An error occurred while deserializing the response from You Verify");
                return new YouVerifyResponse { Success = error.Success, StatusCode = error.StatusCode, Message = error.Message };
            }
            var response = await request.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<YouVerifyResponse>(response) ?? throw new HttpRequestException("An error occured while serializing the response from You Verify");
        }
    }
}
