using SharedModule.DTOs.AddressDTOs;

namespace UserModule.DTOs.ProducerDTOs
{
    /// <summary>
    /// Represents the details of a producer in the system.
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
    public record GetProducerDetailDTO(
        Guid Id,
        string Name,
        string FirstName,
        string LastName,
        string MiddleName,
        string Email,
        string PhoneNumber,
        bool IsEmailVerified,
        bool IsPremium,
        AddressDetail? Address,
        bool IsVerified,
        string VerificationStatus,
        bool IsBlacklisted,
        bool IsDeleted,
        string? Bio = ""
        );
}
