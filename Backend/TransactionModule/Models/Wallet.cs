using SharedModule;
using SharedModule.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionModule.Models
{
    public class Wallet : BaseEntity
    {
        //[Key]
        //public Guid Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0.00m;
        [Column(TypeName = "decimal(18,2)")]
        public decimal LockedBalance { get; set; } = 0.00m;
        public Currency Currency { get; set; } = Currency.NAIRA;
        public string CurrencySymbol => Currency switch
        {
            Currency.NAIRA => "₦",
            Currency.USD => "$",
            Currency.EUR => "€",
            Currency.GBP => "£",
            _ => "₦"
        };
        [ForeignKey("User")]
        public Guid UserId { get; set; }
    }

}
