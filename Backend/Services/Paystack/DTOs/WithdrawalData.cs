namespace Services.Paystack.DTOs
{
    public class WithdrawalData
    {
        public string TransferCode { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }
}
