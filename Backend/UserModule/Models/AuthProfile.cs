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
        public DateTimeOffset LastLoginAt { get; private set; } = DateTime.UtcNow;
        public DateOnly LastLoginDate => DateOnly.FromDateTime(LastLoginAt.UtcDateTime);
        public TimeOnly LastLoginTime => TimeOnly.FromDateTime(LastLoginAt.UtcDateTime);
        public DateTimeOffset? LastLogoutAt { get; set; }
        public DateOnly? LastLogoutDate => LastLogoutAt.HasValue ? DateOnly.FromDateTime(LastLogoutAt.Value.UtcDateTime) : null;
        public TimeOnly? LastLogoutTime => LastLogoutAt.HasValue ? TimeOnly.FromDateTime(LastLogoutAt.Value.UtcDateTime) : null;
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
    }
}
