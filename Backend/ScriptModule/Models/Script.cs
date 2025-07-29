using ScriptModule.Enums;
using SharedModule.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScriptModule.Models
{
    /// <summary>
    /// Defines the script entity which contains all the details of a script
    /// </summary>
    public class Script : BaseEntity
    {
        [Column(TypeName = "Nvarchar(100)")]
        public required string Title { get; set; }
        [Column(TypeName = "Nvarchar(60)")]
        public required string Genre { get; set; }
        public required string Logline { get; set; }
        public required string Synopsis { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }
        public required string Path { get; set; }
        public required string Url { get; set; }
        public DateTimeOffset UploadedOn { get; set; } = DateTime.UtcNow;
        public DateOnly UploadedDate => DateOnly.FromDateTime(UploadedOn.UtcDateTime);
        public TimeOnly UploadedTime => TimeOnly.FromDateTime(UploadedOn.UtcDateTime);
        public Currency Currency { get; set; } = Currency.NAIRA;
        public string CurrencySymbol => Currency switch
        {
            Currency.NAIRA => "₦",
            Currency.USD => "$",
            Currency.EUR => "€",
            Currency.GBP => "£",
            _ => "₦"
        };
        public bool IsScriptRegistered { get; set; }
        public string? RegistrationBody { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? Image { get; set; }
        public string? CopyrightNumber { get; set; }
        public IPDealType? OwnershipRights { get; set; }
        public string? ProofUrl { get; set; }
        [ForeignKey("Writer")]
        public Guid? WriterId { get; set; }
        public ScriptStatus Status { get; set; } = ScriptStatus.Available;
        //public List<SharedScript> SharedScripts { get; set; } = [];
    }
}
