using Microsoft.Extensions.Configuration;
using Services.Paystack.DTOs;
using System.Text;
using System.Text.Json;

namespace Services.Paystack
{
    public class PaystackService : IPaystackService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;
        private readonly string _baseUrl = "https://api.paystack.co";

        public PaystackService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _secretKey = configuration["Paystack:SecretKey"];
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _secretKey);
        }

        public async Task<PaymentInitResponse> InitializePaymentAsync(PaymentInitRequest request)
        {
            var payload = new
            {
                email = request.Email,
                amount = (int)(request.Amount * 100), // Convert to kobo
                currency = request.Currency,
                reference = request.Reference ?? GenerateReference(),
                callback_url = request.CallbackUrl,
                metadata = request.Metadata
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/transaction/initialize", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PaymentInitResponse>(responseContent,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public async Task<PaymentVerifyResponse> VerifyPaymentAsync(string reference)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/transaction/verify/{reference}");
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PaymentVerifyResponse>(responseContent,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public async Task<CreateRecipientResponse> CreateRecipientAsync(CreateRecipientRequest request)
        {
            var payload = new
            {
                type = request.Type,
                name = request.Name,
                account_number = request.AccountNumber,
                bank_code = request.BankCode,
                currency = request.Currency
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/transferrecipient", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CreateRecipientResponse>(responseContent,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public async Task<WithdrawalResponse> InitiateWithdrawalAsync(WithdrawalRequest request)
        {
            var payload = new
            {
                source = "balance",
                amount = (int)(request.Amount * 100), // Convert to kobo
                recipient = request.RecipientCode,
                reason = request.Reason,
                currency = request.Currency,
                reference = GenerateReference()
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/transfer", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<WithdrawalResponse>(responseContent,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        //public async Task<List<BankInfo>> GetBanksAsync()
        //{
        //    var response = await _httpClient.GetAsync($"{_baseUrl}/bank?country=nigeria");
        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    var bankResponse = JsonSerializer.Deserialize<dynamic>(responseContent);
        //    // Parse and return bank list
        //    return new List<BankInfo>(); // Implement parsing
        //}

        private string GenerateReference()
        {
            return $"TXN_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString()[..8]}";
        }
    }
}




