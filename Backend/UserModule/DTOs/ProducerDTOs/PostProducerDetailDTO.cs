using SharedModule.DTOs.AddressDTOs;
using UserModule.DTOs.DocumentDTOs;

namespace UserModule.DTOs.ProducerDTOs
{
    /// <summary>
    /// Defines the details of a producer to be posted or updated.
    /// </summary>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    /// <param name="MiddleName"></param>
    /// <param name="Email"></param>
    /// <param name="Password"></param>
    /// <param name="PhoneNumber"></param>
    /// <param name="Role"></param>
    /// <param name="IsPremium"></param>
    /// <param name="AddressDetail"></param>
    /// <param name="VerificationDocument"></param>
    /// <param name="Bio"></param>
    public record PostProducerDetailDTO(
        string FirstName,
        string LastName,
        string MiddleName,
        string Email,
        string Password,
        string PhoneNumber,
        string Role,
        bool IsPremium,
        AddressDetail AddressDetail,
        PostDocumentDetailDTO VerificationDocument,
        string? Bio = ""
        );
}
