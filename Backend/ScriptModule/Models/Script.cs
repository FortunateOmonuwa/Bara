using ScriptModule.Enums;
using SharedModule.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScriptModule.Models
{
    /// <summary>
    /// Defines the script entity which contains all the details of a script
    /// </summary>
    public class Script : BaseEntity
    {
        //[Key]
        //public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public required string Logline { get; set; }
        public required string Synopsis { get; set; }

        public required double Price { get; set; }
        public bool IsScriptRegistered { get; set; }
        public string? RegistrationBody { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? ProofUrl { get; set; }
        [ForeignKey("Writer")]
        public Guid WriterId { get; set; }
        public ScriptStatus Status { get; set; } = ScriptStatus.Available;
        public List<SharedScript> SharedScripts { get; set; } = [];
    }
}
