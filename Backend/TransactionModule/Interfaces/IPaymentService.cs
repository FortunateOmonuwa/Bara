using SharedModule.Utils;

namespace TransactionModule.Interfaces
{
    public interface IPaymentService
    {
        Task<ResponseDetail<string>> InitiateFundingAsync(Guid userId, decimal amount);
        Task<ResponseDetail<bool>> VerifyAndProcessPaymentAsync(string reference);
        Task<ResponseDetail<bool>> ProcessScriptPurchaseAsync(Guid producerId, Guid writerId, Guid scriptId, decimal amount);
        Task<ResponseDetail<bool>> InitiateWithdrawalAsync(Guid userId, decimal amount, Guid bankAccountId);
        Task<ResponseDetail<bool>> AddBankAccountAsync(Guid userId, string accountNumber, string bankCode);
    }
}
