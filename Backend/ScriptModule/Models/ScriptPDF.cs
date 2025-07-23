using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScriptModule.Models
{
    public class ScriptPDF
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Script")]
        public Guid ScriptID { get; set; }
        public required string Name { get; set; }
        public required string Path { get; set; }
        public string Url { get; set; }
        public DateTimeOffset UploadedOn { get; set; } = DateTime.UtcNow;
        public DateOnly UploadedDate => DateOnly.FromDateTime(UploadedOn.UtcDateTime);
        public TimeOnly UploadedTime => TimeOnly.FromDateTime(UploadedOn.UtcDateTime);
    }
}
