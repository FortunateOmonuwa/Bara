using ScriptModule.Enums;
using SharedModule;
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
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(60)")]
        public required string Name { get; set; }
        [DataType(DataType.Text), Column(TypeName = "Nvarchar(200)")]
        public required string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal MinPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxPrice { get; set; }
        public Currency Currency { get; set; } = Currency.NAIRA;
        public string CurrencySymbol => Currency switch
        {
            Currency.NAIRA => "₦",
            Currency.USD => "$",
            Currency.EUR => "€",
            Currency.GBP => "£",
            _ => "₦"
        };
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
        public Guid ScriptWriterId { get; set; }
        public virtual Writer? ScriptWriter { get; set; }
    }
}
