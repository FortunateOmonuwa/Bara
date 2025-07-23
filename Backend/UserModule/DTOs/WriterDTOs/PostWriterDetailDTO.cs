using SharedModule.DTOs.AddressDTOs;
using UserModule.DTOs.DocumentDTOs;
using UserModule.DTOs.ServiceDTOs;

namespace UserModule.DTOs.WriterDTOs
{
    /// <summary>
    /// Represents the details required to post a new writer's information.
    /// </summary>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    /// <param name="MiddleName"></param>
    /// <param name="Email"></param>
    /// <param name="PhoneNumber"></param>
    /// <param name="Bio"></param>
    /// <param name="Role"></param>
    /// <param name="IsPremium"></param>
    /// <param name="AddressDetail"></param>
    /// <param name="VerificationDocument"></param>
    public record PostWriterDetailDTO(
        string FirstName,
        string LastName,
        string MiddleName,
        string Email,
        string Password,
        string PhoneNumber,
        string Bio,
        string Role,
        bool IsPremium,
        AddressDetail AddressDetail,
        PostDocumentDetailDTO VerificationDocument,
        PostServiceDetailDTO? PostServiceDetail
    );
}
