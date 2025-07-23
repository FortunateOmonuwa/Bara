using Newtonsoft.Json;
using System.Text;

namespace Services.ExternalAPI_Integration
{

    public class ExternalApiIntegrationService(IHttpClientFactory clientFactory)
    {
        private readonly IHttpClientFactory httpClient = clientFactory;

        public StringContent SerializeReqBody(object reqBody)
        {
            var json = JsonConvert.SerializeObject(reqBody);
            var result = new StringContent(json, Encoding.UTF8, "application/json");
            return result;
        }

        public async Task<HttpResponseMessage> SendPostRequest(StringContent jsonBody, string url, Dictionary<string, string>? keyValuePairs = null, string clientName = "default")
        {
            var client = httpClient.CreateClient(clientName);

            if (keyValuePairs != null)
            {
                foreach (var kvp in keyValuePairs)
                {
                    if (!client.DefaultRequestHeaders.Contains(kvp.Key))
                        client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                }
            }

            var response = await client.PostAsync(url, jsonBody);
            return response;
        }
    }
}
