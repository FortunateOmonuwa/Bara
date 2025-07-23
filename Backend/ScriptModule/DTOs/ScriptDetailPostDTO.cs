using ScriptModule.Enums;
using ScriptModule.Models;

namespace ScriptModule.DTOs
{
    public class ScriptDetailPostDTO
    {
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public required string Logline { get; set; }
        public required string Synopsis { get; set; }
        public required decimal Price { get; set; }
        public bool IsScriptRegistered { get; set; }
        public string? RegistrationBody { get; set; }
        public string? Image { get; set; }
        public required ScriptPDF File { get; set; }
        public string? CopyrightNumber { get; set; }
        public IPDealType OwnershipRights { get; set; }
        public string? ProofUrl { get; set; }
    }
}
