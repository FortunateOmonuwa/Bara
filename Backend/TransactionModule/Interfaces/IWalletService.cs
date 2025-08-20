namespace TransactionModule.Interfaces
{
    public interface IWalletService
    {
        Task<string> FundWalletAsync(Guid userId, decimal amount, string email);
        Task<bool> ProcessPaymentCallbackAsync(string reference);
        Task<bool> WithdrawFundsAsync(Guid userId, decimal amount, Guid bankAccountId);
        Task<bool> ProcessScriptPaymentAsync(Guid producerId, Guid writerId, Guid scriptId, decimal amount);
        Task<decimal> GetWalletBalanceAsync(Guid userId);
    }
}
