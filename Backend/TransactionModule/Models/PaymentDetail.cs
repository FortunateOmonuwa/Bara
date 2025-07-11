namespace TransactionModule.Models
{
    public class PaymentDetail
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public string PaymentMethodToken { get; set; }
        public string CardBrand { get; set; }
        public string Last4 { get; set; }
    }

}
