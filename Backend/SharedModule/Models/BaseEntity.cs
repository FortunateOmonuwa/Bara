using System.ComponentModel.DataAnnotations;

namespace SharedModule.Models
{
    /// <summary>
    /// This class contains properties that might be needed for different models
    /// </summary>
    public class BaseEntity
    {
        [Key]
        public virtual Guid Id { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateOnly DateCreated => DateOnly.FromDateTime(CreatedAt);
        public TimeOnly TimeCreated => TimeOnly.FromDateTime(CreatedAt);
        public DateTime? ModifiedAt { get; set; }
        public DateOnly? DateModified => ModifiedAt.HasValue ? DateOnly.FromDateTime(ModifiedAt.Value) : null;
        public TimeOnly? TimeModified => ModifiedAt.HasValue ? TimeOnly.FromDateTime(ModifiedAt.Value) : null;
    }
}
