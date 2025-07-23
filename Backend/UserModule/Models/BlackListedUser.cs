using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserModule.Models
{
    public class BlackListedUser
    {
        //[JsonIgnore]
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public required string Name { get; set; }
        public string? Reason { get; set; }
        public DateTimeOffset BlackListedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateOnly BlackListedAtDate => DateOnly.FromDateTime(BlackListedAt.UtcDateTime);
        public TimeOnly BlackListedAtTime => TimeOnly.FromDateTime(BlackListedAt.UtcDateTime);
    }
}
