using Microsoft.AspNetCore.Http;

namespace UserModule.DTOs.DocumentDTOs
{
    /// <summary>
    /// Represents the details required to post a document for user verification.
    /// </summary>
    /// <param name="Document"> Represents the uploaded document</param>
    public record PostDocumentDetailDTO(
            IFormFile Document
        );
}
