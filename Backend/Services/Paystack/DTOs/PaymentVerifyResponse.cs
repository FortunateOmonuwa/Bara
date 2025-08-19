namespace Services.Paystack.DTOs
{
    public class PaymentVerifyResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public PaymentVerificationData Data { get; set; }
    }
}
