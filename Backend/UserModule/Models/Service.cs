using ScriptModule.Enums;
using SharedModule.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserModule.Models
{
    /// <summary>
    /// Defines all the services rendered by a Writer user in the application, such as script editing, proofreading, etc.
    /// </summary>
    public class Services : BaseEntity
    {
        //[Key]
        //public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required double MinPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double MaxPrice { get; set; }
        public IPDealType IPDealType { get; set; }
        /// <summary>
        /// If the IP is shared, this defines the percentage share for the producer.
        /// The writer gets (100 - ProducerSharePercentage)%.
        /// Only applicable if IPArrangement == SharedRights.
        /// </summary>
        [Range(1, 99)]
        public int SharePercentage { get; set; }
        public PaymentType PaymentType { get; set; }
        public List<string> Genre { get; set; } = [];
        [ForeignKey(nameof(ScriptWriter))]
        public Guid WriterId { get; set; }
        public virtual Writer? ScriptWriter { get; set; }
    }
}
