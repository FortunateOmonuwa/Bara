using SharedModule.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionModule.Models
{
    public class Wallet : BaseEntity
    {
        //[Key]
        //public Guid Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double Balance { get; set; } = 0.00;
        [Column(TypeName = "decimal(18,2)")]
        public double LockedBalance { get; set; } = 0.00;
        public string Currency { get; set; } = "NGN";
    }

}
