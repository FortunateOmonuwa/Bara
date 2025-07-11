using ScriptModule.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UserModule.Models
{
    /// <summary>
    /// Defines a Writer user in the application, which is a specialized type of User that can create and manage scripts.
    /// </summary>
    public class Writer : User
    {
        //[Key]
        //public Guid Id { get; set; }
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(200)")]
        public override required string Bio { get; set; } = string.Empty;
        /// <summary>
        /// The list of services provided by the writer, such as script editing, proofreading, etc.
        /// </summary>
        public List<Services> Services { get; set; } = [];
        public List<Script> Scripts { get; set; } = [];
        /// <summary>
        /// The list of all the scripts shared between the writer and producers
        /// </summary>
        public List<SharedScript> SharedScripts { get; set; } = [];
        //public List<Transaction> Transactions { get; set; } = [];
        //public List<ScriptWritingPostApplicant> Applicarions { get; set; } = [];
    }
}
