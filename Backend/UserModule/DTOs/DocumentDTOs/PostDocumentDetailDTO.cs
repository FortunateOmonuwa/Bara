using Microsoft.AspNetCore.Http;
using UserModule.Enums;

namespace UserModule.DTOs.DocumentDTOs
{
    /// <summary>
    /// Represents the details required to post a document for user verification.
    /// </summary>
    /// <param name="Document"> Represents the uploaded document</param>
    public record class PostDocumentDetailDTO
    {
        public required DocumentType Type { get; init; }
        public required string VerificationNumber { get; init; }
        public required IFormFile Document { get; init; }
    }
}
