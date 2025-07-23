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
            BaseUrl = appSettings.Value.YouVerifyBaseUrl;
            BVN_URL = $"{appSettings.Value.YouVerify_BVN_VerificationUrl}";
            Passport_URL = $"{appSettings.Value.YouVerify_International_Passport_VerificationUrl}";
            NIN_URL = $"{appSettings.Value.YouVerify_NIN_VerificationUrl}";
            Drivers_License_URL = $"{appSettings.Value.YouVerify_Drivers_License_VerificationUrl}";

            this.apiService = apiService;
            secrets = appSecrets.Value;
            settings = appSettings.Value;
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

            var body = apiService.SerializeReqBody(details);
            //var header = new Dictionary<string, string>
            //{
            //    { "token", secrets.YouVerifyLiveAPIKEY },
            //};
            var request = await apiService.SendPostRequest(body, url, null, "YouVerify");
            if (!request.IsSuccessStatusCode)
            {
                var errorResponse = await request.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<YouVerifyResponse>(errorResponse) ?? throw new HttpRequestException("An error occured while verifying the document");
            }
            var response = await request.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<YouVerifyResponse>(response) ?? throw new HttpRequestException("An error occured while serializing the response from You Verify");
        }
    }
}
