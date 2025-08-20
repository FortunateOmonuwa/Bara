namespace Services.Paystack.DTOs
{
    public class PaymentVerificationData
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public string Domain { get; set; }
        public string Gateway { get; set; }
        //public Customer Customer { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }
}
