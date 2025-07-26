using SharedModule.DTOs.AddressDTOs;
using UserModule.DTOs.DocumentDTOs;
using UserModule.DTOs.ServiceDTOs;
using UserModule.Enums;

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
    public record class PostWriterDetailDTO
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public string MiddleName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string PhoneNumber { get; init; }
        public required string Bio { get; init; }
        public required Gender Gender { get; init; }
        public required DateOnly DateOfBirth { get; init; }
        public bool IsPremiumMember { get; init; }
        public required AddressDetail AddressDetail { get; init; }
        public required PostDocumentDetailDTO VerificationDocument { get; init; }
        public List<PostServiceDetailDTO>? PostServiceDetail { get; init; }
    }

}
