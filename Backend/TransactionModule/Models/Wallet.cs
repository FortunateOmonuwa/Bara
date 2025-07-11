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
        public string Currency { get; set; } = "NGN";
    }

}
