using SharedModule.DTOs.AddressDTOs;
using TransactionModule.DTOs;
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
    public class GetWriterDetailDTO
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? MiddleName { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Bio { get; init; }
        public bool IsEmailVerified { get; init; }
        public bool IsPremium { get; init; }
        public AddressDetail? Address { get; init; }
        public bool IsVerified { get; init; }
        public string VerificationStatus { get; init; }
        public bool IsBlacklisted { get; init; }
        public string Role { get; init; }
        public List<GetServiceDetailDTO>? Services { get; init; }
        public GetWalletDetailDTO Wallet { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateOnly DateCreated { get; init; }
        public TimeOnly TimeCreated { get; init; }
        public DateTimeOffset? ModifiedAt { get; init; }
        public DateOnly? DateModified { get; init; }
        public TimeOnly? TimeModified { get; init; }
    }


}
