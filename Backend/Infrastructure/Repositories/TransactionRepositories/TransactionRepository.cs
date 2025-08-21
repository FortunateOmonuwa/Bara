using Infrastructure.DataContext;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.MailingService;
using Services.Paystack;
using Services.Paystack.DTOs;
using Services.SignalR;
using SharedModule.Models;
using SharedModule.Utils;
using TransactionModule.DTOs.TransactionDTOs;
using TransactionModule.Enums;
using TransactionModule.Interfaces;
using TransactionModule.Models;

namespace Infrastructure.Repositories.TransactionRepositories
{
    public class TransactionRepository : ITransactionService
    {
        private readonly IPaystackService paystack;
        private readonly LogHelper<TransactionRepository> logHelper;
        private readonly ILogger<TransactionRepository> logger;
        private readonly BaraContext dbContext;
        private readonly IMailService mailer;
        private readonly IHubContext<NotificationHub> notificationHub;
        public TransactionRepository(IPaystackService paystackService, LogHelper<TransactionRepository> logHelper,
            ILogger<TransactionRepository> logger, BaraContext context, IMailService mailer, IHubContext<NotificationHub> notificationHub)
        {
            paystack = paystackService;
            this.logHelper = logHelper;
            this.logger = logger;
            dbContext = context;
            this.mailer = mailer;
            this.notificationHub = notificationHub;
        }
        public async Task<ResponseDetail<object>> InitiateTransactionAsync(TransactionInitDTO data)
        {
            try
            {
                var user = await dbContext.Users.Where(x => x.Id == data.UserId)
                        .Select(x => new
                        {
                            x.Id,
                            x.Email,
                            x.AuthProfile.FullName,
                            walletId = x.Wallet.Id
                        }).FirstOrDefaultAsync();
                if (user == null)
                {
                    logger.LogInformation($"User with ID {data.UserId} not found while initiating transaction.");
                    return ResponseDetail<object>.Failed("User not found", 404);
                }

                var transaction = new PaymentTransaction
                {
                    UserId = user.Id,
                    UserFullName = user.FullName,
                    Amount = data.Amount,
                    Status = TransactionStatus.Initiated,
                    TransactionType = TransactionType.WalletFunding,
                    WalletID = user.walletId
                };

                await dbContext.Transactions.AddAsync(transaction);
                var dbRes = await dbContext.SaveChangesAsync();
                if (dbRes > 1)
                {
                    var paymentInitRequest = new PaymentInitRequest
                    {
                        Amount = data.Amount,
                        Email = user.Email,
                        Currency = Currency.NAIRA.ToString(),
                        TransactionId = transaction.Id,
                        UserId = user.Id,
                        CustomerName = user.FullName,
                        Metadata = new Dictionary<string, object>
                        {
                            { "UserId", user.Id },
                            { "CustomerName", user.FullName },
                            { "Email", user.Email },
                            { "TransactionId", transaction.Id }
                        },
                    };
                    var paymentResponse = await paystack.InitializePaymentAsync(paymentInitRequest);
                    if (paymentResponse.Status)
                    {
                        transaction.ReferenceId = paymentResponse.Data.Reference;
                        transaction.Status = TransactionStatus.Pending;
                        transaction.AccessCode = paymentResponse.Data.AccessCode;
                        await dbContext.SaveChangesAsync();
                        return ResponseDetail<object>.Successful(new
                        {
                            transaction.Id,
                            paymentUrl = paymentResponse.Data.AuthorizationUrl,
                        }, "Transaction initiated successfully");
                    }
                    else
                    {
                        transaction.Status = TransactionStatus.Failed;
                        await dbContext.SaveChangesAsync();
                        logger.LogError($"Failed to initialize payment for user {user.FullName}. Error: {paymentResponse.Message}");
                        return ResponseDetail<object>.Failed(paymentResponse.Message, 500);
                    }
                }
                else
                {
                    logger.LogError($"Failed to save transaction for user {user.FullName}. Database error.");
                    return ResponseDetail<object>.Failed("Failed to initiate transaction", 500);
                }
            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, $"While creating a transaction for {data.UserId}");
                return ResponseDetail<object>.Failed("Failed to initiate transaction", 500, ex.Message);
            }
        }

        public Task<ResponseDetail<bool>> InitiateWithdrawalAsync(Guid userId, decimal amount, Guid bankAccountId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<bool>> ProcessScriptPurchaseAsync(Guid producerId, Guid writerId, Guid scriptId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDetail<bool>> VerifyTransactionAsync(Guid userId, string reference)
        {
            //var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var result = await (
                                from t in dbContext.Transactions
                                join u in dbContext.Users on t.UserId equals u.Id
                                where t.UserId == userId && t.ReferenceId == reference
                                select new
                                {
                                    u.Id,
                                    Transaction = t,
                                    u.Wallet,
                                    u.PaymentDetails
                                }
                            ).FirstOrDefaultAsync();


                if (result is null)
                {
                    logger.LogInformation($"Transaction with reference {reference} for user with ID {userId} not found while verifying transaction.");
                    return ResponseDetail<bool>.Failed("Transaction not found", 404);
                }

                var userTransaction = result.Transaction;
                if (userTransaction == null)
                {
                    return ResponseDetail<bool>.Failed("Transaction not found", 404);
                }

                var verifyReq = await paystack.VerifyPaymentAsync(userTransaction.ReferenceId);
                if (verifyReq.Status && verifyReq.Data != null)
                {
                    if (verifyReq.Data.Status == "success" && verifyReq.Data.Amount == userTransaction.Amount && userTransaction.Status is TransactionStatus.Completed)
                    {
                        return ResponseDetail<bool>.Successful(true, "Transaction already verified");
                    }
                    else if (verifyReq.Data.Status == "success" && verifyReq.Data.Amount == userTransaction.Amount)
                    {
                        var paymentDetail = result.PaymentDetails.FirstOrDefault(x => x.AuthorizationCode == verifyReq.Data.Authorization.AuthorizationCode && x.UserId == userId);
                        if (paymentDetail is null)
                        {
                            var newPaymentDetail = new PaymentDetail
                            {
                                UserId = userId,
                                AuthorizationCode = verifyReq.Data.Authorization.AuthorizationCode,
                                Last4 = verifyReq.Data.Authorization.Last4,
                                CardType = verifyReq.Data.Authorization.CardType,
                                ExpMonth = verifyReq.Data.Authorization.ExpMonth,
                                Bank = verifyReq.Data.Authorization.Bank,
                                CountryCode = verifyReq.Data.Authorization.CountryCode,
                                CustomerCode = verifyReq.Data.Customer.CustomerCode,
                                CustomerId = verifyReq.Data.Customer.Id.ToString(),
                                ExpYear = verifyReq.Data.Authorization.ExpYear,
                                Reusable = verifyReq.Data.Authorization.Reusable
                            };

                            result.PaymentDetails.Add(newPaymentDetail);
                        }
                        userTransaction.Status = TransactionStatus.Completed;
                        userTransaction.CompletedAt = DateTimeOffset.UtcNow;
                        userTransaction.CreatedAt = verifyReq.Data.CreatedAt.HasValue
                           ? DateTimeOffset.Parse(verifyReq.Data.CreatedAt.Value.ToString())
                           : userTransaction.CreatedAt;
                        result.Wallet.AvailableBalance += userTransaction.Amount;
                        result.Wallet.TotalBalance += userTransaction.Amount;
                        userTransaction.GatewayResponse = verifyReq.Data.GatewayResponse;
                        userTransaction.PaymentMethod = verifyReq.Data.Channel;
                        userTransaction.Fee = verifyReq.Data.Fees;
                        userTransaction.Notes = verifyReq.Message;
                        userTransaction.ModifiedAt = DateTimeOffset.UtcNow;

                        dbContext.Transactions.Update(userTransaction);
                        dbContext.Wallets.Update(result.Wallet);
                        await dbContext.SaveChangesAsync();

                        await notificationHub.Clients.User(userId.ToString())
                            .SendAsync("WalletUpdated", new
                            {
                                Balance = result.Wallet.AvailableBalance,
                                Total = result.Wallet.TotalBalance
                            });

                        return ResponseDetail<bool>.Successful(true, "Transaction verified successfully");
                    }
                    else
                    {
                        userTransaction.Status = verifyReq.Data.Status switch
                        {
                            "failed" => TransactionStatus.Failed,
                            "abandoned" => TransactionStatus.Abandoned,
                            "cancelled" => TransactionStatus.Cancelled,
                            _ => TransactionStatus.Failed
                        };
                        userTransaction.ModifiedAt = DateTimeOffset.UtcNow;
                        dbContext.Transactions.Update(userTransaction);
                        await dbContext.SaveChangesAsync();
                        return ResponseDetail<bool>.Failed("Transaction verification failed", 400);
                    }
                }
                else
                {
                    logger.LogError($"Failed to verify transaction for user {userId}. Error: {verifyReq.Message}");
                    return ResponseDetail<bool>.Failed(verifyReq.Message, 500);
                }

            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, $"while verifying transaction for {userId}");
                return ResponseDetail<bool>.Failed("An error occured while verifying the transaction", 500, ex.Message);
            }
        }
    }
}
