using ScriptModule.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScriptModule.Models
{
    /// <summary>
    /// Defines the shared script between writer and producer after an agreement is made
    /// </summary>
    public class SharedScript
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(Script))]
        public required Guid ScriptId { get; set; }
        public Script? Script { get; set; }
        [ForeignKey("Writer")]
        public required Guid WriterId { get; set; }
        [ForeignKey("Producer")]
        public required Guid ProducerId { get; set; }
        public DateTime SharedAt { get; set; } = DateTime.UtcNow;
        public DateOnly SharedDate => DateOnly.FromDateTime(SharedAt);
        public TimeOnly SharedTime => TimeOnly.FromDateTime(SharedAt);
        public string? EncryptedScriptUrl { get; set; }

        public ScriptDeliveryStatus Status { get; set; } = ScriptDeliveryStatus.Pending;
    }

}
