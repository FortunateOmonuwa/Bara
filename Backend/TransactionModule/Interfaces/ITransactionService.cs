using SharedModule.Utils;
using TransactionModule.DTOs.TransactionDTOs;

namespace TransactionModule.Interfaces
{
    public interface ITransactionService
    {
        Task<ResponseDetail<object>> InitiateTransactionAsync(TransactionInitDTO data);
        Task<ResponseDetail<bool>> VerifyTransactionAsync(Guid userId, string reference);
        //Task<ResponseDetail<bool>> ProcessScriptPurchaseAsync(Guid producerId, Guid writerId, Guid scriptId, decimal amount);
        Task<ResponseDetail<bool>> InitiateWithdrawalAsync(Guid userId, decimal amount, Guid bankAccountId);
    }
}
