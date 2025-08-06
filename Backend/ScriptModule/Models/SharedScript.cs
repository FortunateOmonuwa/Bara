using ScriptModule.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScriptModule.Models
{
    /// <summary>
    /// Defines the shared script between a writer and a producer after a formal agreement is made.
    /// Captures delivery status, shared time, and access permissions.
    /// </summary>
    public class SharedScript
    {
        /// <summary>
        /// The unique identifier for this shared script record.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The unique identifier of the shared script.
        /// </summary>
        [ForeignKey(nameof(Script))]
        public required Guid ScriptId { get; set; }

        /// <summary>
        /// Navigation property to the original script being shared.
        /// </summary>
        public Script? Script { get; set; }

        /// <summary>
        /// The unique identifier of the writer sharing the script.
        /// </summary>
        [ForeignKey("Writer")]
        public required Guid WriterId { get; set; }

        /// <summary>
        /// The unique identifier of the producer receiving the script.
        /// </summary>
        [ForeignKey("Producer")]
        public required Guid ProducerId { get; set; }

        /// <summary>
        /// The exact UTC timestamp when the script was shared.
        /// </summary>
        public DateTimeOffset SharedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date part of the shared timestamp.
        /// </summary>
        public DateOnly SharedDate => DateOnly.FromDateTime(SharedAt.UtcDateTime);

        /// <summary>
        /// The time part of the shared timestamp.
        /// </summary>
        public TimeOnly SharedTime => TimeOnly.FromDateTime(SharedAt.UtcDateTime);

        /// <summary>
        /// A secure, possibly encrypted, URL for accessing the shared script file.
        /// </summary>
        public string? EncryptedScriptUrl { get; set; }

        /// <summary>
        /// The current delivery or access status of the shared script.
        /// </summary>
        public ScriptDeliveryStatus Status { get; set; } = ScriptDeliveryStatus.Pending;
    }
}
