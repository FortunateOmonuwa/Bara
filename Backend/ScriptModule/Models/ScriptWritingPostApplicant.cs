using System.ComponentModel.DataAnnotations.Schema;

namespace ScriptModule.Models
{
    public class ScriptWritingPostApplicant
    {
        public Guid WriterId { get; set; }
        public required string WriterName { get; set; } = string.Empty;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public DateOnly AppliedDate => DateOnly.FromDateTime(AppliedAt);
        public TimeOnly AppliedTime => TimeOnly.FromDateTime(AppliedAt);
        [ForeignKey(nameof(ScriptWritingPost))]
        public Guid ScriptWritingPostByProducerId { get; set; }
        public ScriptWritingPostByProducer? ScriptWritingPost { get; set; }
    }
}
