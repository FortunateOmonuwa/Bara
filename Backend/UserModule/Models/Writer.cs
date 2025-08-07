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
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(300)")]
        public override required string Bio { get; set; }

        public bool IsPremiumMember { get; set; }
        /// <summary>
        /// The list of services provided by the writer, such as script editing, proofreading, etc.
        /// </summary>
        public List<Service> Services { get; set; } = [];
        public List<Script> Scripts { get; set; } = [];
        //public List<ScriptWritingPostApplicant> Applicarions { get; set; } = [];
    }
}
