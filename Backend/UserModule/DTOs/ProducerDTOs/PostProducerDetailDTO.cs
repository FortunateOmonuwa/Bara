using SharedModule.DTOs.AddressDTOs;
using UserModule.DTOs.DocumentDTOs;
using UserModule.Enums;

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
    public record PostProducerDetailDTO
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public string MiddleName { get; init; } = string.Empty;
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string PhoneNumber { get; init; }
        public required DateOnly DateOfBirth { get; init; }
        public required Gender Gender { get; init; }
        public required AddressDetail AddressDetail { get; init; }
        public required PostDocumentDetailDTO VerificationDocument { get; init; }
        public string Bio { get; init; } = "";
    }

}
