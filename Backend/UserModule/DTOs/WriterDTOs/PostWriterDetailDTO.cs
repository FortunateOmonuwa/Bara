using SharedModule.DTOs.AddressDTOs;
using UserModule.DTOs.DocumentDTOs;
using UserModule.DTOs.ServiceDTOs;
using UserModule.Enums;

namespace UserModule.DTOs.WriterDTOs
{
    /// <summary>
    /// Represents the details required to register a new writer in the system.
    /// </summary>
    public record class PostWriterDetailDTO
    {
        /// <summary>
        /// The writer's first name.
        /// </summary>
        public required string FirstName { get; init; }

        /// <summary>
        /// The writer's last name.
        /// </summary>
        public required string LastName { get; init; }

        /// <summary>
        /// The writer's middle name, if available.
        /// </summary>
        public string MiddleName { get; init; }

        /// <summary>
        /// The writer's email address.
        /// </summary>
        public required string Email { get; init; }

        /// <summary>
        /// The writer's login password.
        /// </summary>
        public required string Password { get; init; }

        /// <summary>
        /// The writer's phone number, including country code.
        /// </summary>
        public required string PhoneNumber { get; init; }

        /// <summary>
        /// A brief biography or profile summary of the writer.
        /// </summary>
        public required string Bio { get; init; }

        /// <summary>
        /// The gender of the writer.
        /// </summary>
        public required Gender Gender { get; init; }

        /// <summary>
        /// The writer's date of birth.
        /// </summary>
        public required DateOnly DateOfBirth { get; init; }

        /// <summary>
        /// Indicates whether the writer is a premium member.
        /// </summary>
        public bool IsPremiumMember { get; init; }

        /// <summary>
        /// The address details of the writer.
        /// </summary>
        public required AddressDetail AddressDetail { get; init; }

        /// <summary>
        /// The verification document submitted by the writer for identity confirmation.
        /// </summary>
        public required PostDocumentDetailDTO VerificationDocument { get; init; }

        /// <summary>
        /// The list of services the writer wants to offer on registration (e.g., editing, proofreading).
        /// </summary>
        public List<PostServiceDetailDTO>? PostServiceDetail { get; init; }
    }
}
