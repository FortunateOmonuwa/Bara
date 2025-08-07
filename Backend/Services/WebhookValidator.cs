using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
    public static class YouVerifyWebhookVerifier
    {
        public static bool IsValidYouVerifySignature(HttpRequest request, string signingKey)
        {
            var headerSignature = request.Headers["x-youverify-signature"].ToString();
            if (string.IsNullOrEmpty(headerSignature) || !headerSignature.StartsWith("sha256="))
                return false;

            var reqSignature = headerSignature["sha256=".Length..];

            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var rawBody = reader.ReadToEnd();
            request.Body.Position = 0;

            var secretBytes = Encoding.UTF8.GetBytes(signingKey);
            var payloadBytes = Encoding.UTF8.GetBytes(rawBody);

            using var hmac = new HMACSHA256(secretBytes);
            var hashBytes = hmac.ComputeHash(payloadBytes);
            var computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedSignature),
                Encoding.UTF8.GetBytes(reqSignature));
        }

        public static bool IsValidYouVerifySignature(string rawBody, string signatureHeader, string signingKey)
        {
            if (string.IsNullOrEmpty(signatureHeader) || !signatureHeader.StartsWith("sha256="))
                return false;

            var reqSignature = signatureHeader["sha256=".Length..];

            var secretBytes = Encoding.UTF8.GetBytes(signingKey);
            var payloadBytes = Encoding.UTF8.GetBytes(rawBody);

            using var hmac = new HMACSHA256(secretBytes);
            var hashBytes = hmac.ComputeHash(payloadBytes);
            var computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedSignature),
                Encoding.UTF8.GetBytes(reqSignature));
        }
    }
}
