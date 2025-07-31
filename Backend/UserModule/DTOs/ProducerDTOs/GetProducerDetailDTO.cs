using SharedModule.DTOs.AddressDTOs;
using UserModule.Enums;

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
    public record GetProducerDetailDTO
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string MiddleName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string PhoneNumber { get; init; } = default!;
        public bool IsEmailVerified { get; init; }
        public AddressDetail? Address { get; init; }
        public bool IsVerified { get; init; }
        public VerificationStatus VerificationStatus { get; init; }
        public bool IsBlacklisted { get; init; }
        public bool IsDeleted { get; init; }
        public string Bio { get; init; }
    }

}
