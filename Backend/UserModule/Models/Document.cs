using SharedModule.Models;
using System.ComponentModel.DataAnnotations;
using UserModule.Enums;

namespace UserModule.Models
{
    /// <summary>
    /// Defines the user verification document that's needed to verify the user account
    /// </summary>
    public class Document : BaseEntity
    {
        //[Key]
        //public Guid Id { get; set; }
        public required string IdentificationNumber { get; set; }
        public required DocumentType DocumentType { get; set; }
        public bool IsVerified { get; set; }
        [DataType(DataType.Upload)]
        public string? DocumentUrl { get; set; }
        public required string Name { get; set; }
        [DataType(DataType.Url)]
        public required string Path { get; set; }
        public string? FileExtension { get; set; }

        public long Size { get; set; }
    }
}
