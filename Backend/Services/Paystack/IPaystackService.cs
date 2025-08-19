using Services.Paystack.DTOs;

namespace Services.Paystack
{
    public interface IPaystackService
    {
        Task<PaymentInitResponse> InitializePaymentAsync(PaymentInitRequest request);
        Task<PaymentVerifyResponse> VerifyPaymentAsync(string reference);
        Task<WithdrawalResponse> InitiateWithdrawalAsync(WithdrawalRequest request);
        Task<CreateRecipientResponse> CreateRecipientAsync(CreateRecipientRequest request);
    }
}
