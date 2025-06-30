using SharedModule.Models;
using System.ComponentModel.DataAnnotations.Schema;
using TransactionModule.Enums;

namespace TransactionModule.Models
{
    public class Escrow : BaseEntity
    {
        //[Key]
        //public Guid Id { get; set; }
        [ForeignKey(nameof(Transaction))]
        public required Guid TransactionId { get; set; }
        public required Transaction Transaction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double Amount { get; set; }
        public EscrowStatus Status { get; set; }
        public DateTime LockedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReleasedAt { get; set; }
        public string? Reason { get; set; }
    }
}
