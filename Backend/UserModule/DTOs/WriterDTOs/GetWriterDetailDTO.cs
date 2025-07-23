using SharedModule.DTOs.AddressDTOs;
using UserModule.DTOs.ServiceDTOs;

namespace UserModule.DTOs.WriterDTOs
{
    /// <summary>
    /// Represents the details of a writer, including personal information, contact details, and verification status.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    /// <param name="MiddleName"></param>
    /// <param name="Email"></param>
    /// <param name="PhoneNumber"></param>
    /// <param name="Bio"></param>
    /// <param name="IsEmailVerified"></param>
    /// <param name="IsPremium"></param>
    /// <param name="Address"></param>
    /// <param name="IsVerified"></param>
    /// <param name="VerificationStatus"></param>
    /// <param name="IsBlacklisted"></param>
    /// <param name="IsDeleted"></param>
    /// <param name="Services"></param>
    public record GetWriterDetailDTO(
        Guid Id,
        string Name,
        string FirstName,
        string LastName,
        string MiddleName,
        string Email,
        string PhoneNumber,
        string Bio,
        bool IsEmailVerified,
        bool IsPremium,
        AddressDetail? Address,
        bool IsVerified,
        string VerificationStatus,
        bool IsBlacklisted,
        bool IsDeleted,
        List<GetServiceDetailDTO>? Services
    );

}
