using Shared.Models;
using SharedModule.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TransactionModule.Models;
using UserModule.Enums;

namespace UserModule.Models
{
    /// <summary>
    /// Defines the base User entity
    /// </summary>
    public class User : BaseEntity
    {
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(60)")]
        public required string FirstName { get; set; }
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(60)")]
        public required string LastName { get; set; }
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(60)")]
        public string MiddleName { get; set; } = "";
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$\r\n", ErrorMessage = "Please enter a valid Email Address"), DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no."), DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(200)")]
        public virtual string Bio { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public string Role { get; set; } = string.Empty;
        public required Gender Gender { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        [ForeignKey(nameof(Address))]
        public Guid AddressId { get; set; }
        public required Address Address { get; set; }
        [ForeignKey(nameof(VerificationDocument))]
        public Guid VerificationDocumentID { get; set; }
        public required Document VerificationDocument { get; set; }
        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;
        public bool IsVerified { get; set; }
        public bool IsBlacklisted { get; set; }
        [ForeignKey(nameof(Wallet))]
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; } = new Wallet();
        public bool IsDeleted { get; set; } = false;
        public List<Transaction> Transactions { get; set; } = [];
        public DateTimeOffset? DeletedAt { get; set; }
        public DateOnly DeletedAtDate => DeletedAt.HasValue ? DateOnly.FromDateTime(DeletedAt.Value.UtcDateTime) : DateOnly.MinValue;
        public TimeOnly DeletedAtTime => DeletedAt.HasValue ? TimeOnly.FromDateTime(DeletedAt.Value.UtcDateTime) : TimeOnly.MinValue;
    }
}
