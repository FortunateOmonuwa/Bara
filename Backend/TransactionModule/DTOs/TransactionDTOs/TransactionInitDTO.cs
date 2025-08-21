namespace TransactionModule.DTOs.TransactionDTOs
{
    public class TransactionInitDTO
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
