using SharedModule.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserModule.Models
{
    /// <summary>
    /// Defines the Authentication profile of a user
    /// </summary>
    public class AuthProfile : BaseEntity
    {
        /// [Key]
        //public Guid Id { get; set; }
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
        //[DataType(DataType.Password)]
        public required string Password { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public string LastLoginDevice { get; set; } = string.Empty;
        public int LoginAttempts { get; set; }
        public required string Role { get; set; }
        public DateTimeOffset? LastLoginAt { get; set; }
        public DateOnly? LastLoginDate => LastLoginAt.HasValue ? DateOnly.FromDateTime(LastLoginAt.Value.UtcDateTime) : null;
        public TimeOnly? LastLoginTime => LastLoginAt.HasValue ? TimeOnly.FromDateTime(LastLoginAt.Value.UtcDateTime) : null;
        public DateTimeOffset? LastLogoutAt { get; set; }
        public DateOnly? LastLogoutDate => LastLogoutAt.HasValue ? DateOnly.FromDateTime(LastLogoutAt.Value.UtcDateTime) : null;
        public TimeOnly? LastLogoutTime => LastLogoutAt.HasValue ? TimeOnly.FromDateTime(LastLogoutAt.Value.UtcDateTime) : null;
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
    }
}
