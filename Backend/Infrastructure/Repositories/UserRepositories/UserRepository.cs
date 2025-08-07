using Infrastructure.DataContext;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.SignalR;
using SharedModule.Utils;
using UserModule.Enums;
using UserModule.Interfaces.UserInterfaces;
using UserModule.Models;

namespace Infrastructure.Repositories.UserRepositories
{
    public class UserRepository : IUserService
    {
        private readonly BaraContext dbContext;
        private readonly LogHelper<UserRepository> logHelper;
        private readonly ILogger<UserRepository> logger;
        private readonly IHubContext<NotificationHub> notificationHub;
        public UserRepository(BaraContext baraContext, LogHelper<UserRepository> logHelper, ILogger<UserRepository> logger, IHubContext<NotificationHub> hubContext)
        {
            dbContext = baraContext;
            this.logHelper = logHelper;
            this.logger = logger;
            notificationHub = hubContext;
        }

        public Task<ResponseDetail<bool>> BlackListUser(Guid userId, string? reason)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<BlackListedUser>> GetBlackListedUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<List<BlackListedUser>>> GetBlackListedUsers(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDetail<bool>> RemoveUserFromBlackList(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDetail<bool>> UpdateUserVerificationStatus(string verificationIdNumber, string dateOfBirth, bool isSuccessful, string firstName, string lastName, string type)
        {
            string name = "";
            try
            {
                var user = await dbContext.User
                    .Include(x => x.VerificationDocument)
                    .Include(x => x.AuthProfile)
                    .FirstOrDefaultAsync(x => x.VerificationDocument.IdentificationNumber == verificationIdNumber);

                var errors = new List<string>();

                if (isSuccessful is false) errors.Add("Validation failed. User verification is unsuccessful. Please try again");
                if (user == null) errors.Add($"User not found with the provided verification ID number {verificationIdNumber}.");

                name = $"{user?.FirstName} {user?.LastName}";
                var dateOfBirthTallies = user.DateOfBirth.ToString("yyyy-MM-dd") == dateOfBirth;
                var nameTallies = user.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) && user.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase);

                if (!dateOfBirthTallies) errors.Add($"The date of birth on your {type.ToUpper()} does not match the provided date of birth at the time of registration.");
                if (!nameTallies) errors.Add(name + $"The name on your {type.ToUpper()}does not match the provided firstname and/or lastname at the time of registration.");
                if (errors.Count > 0)
                {
                    user.VerificationStatus = VerificationStatus.Failed;
                    user.ModifiedAt = DateTimeOffset.UtcNow;
                    await notificationHub.Clients.User(user.Id.ToString()).SendAsync("KycFailed", new
                    {
                        message = $"Your KYC was verified unsuccessful... {string.Join(" |\n ", errors)}",
                        time = DateTime.UtcNow
                    });

                    return ResponseDetail<bool>.Failed(false, string.Join(" |\n ", errors));
                }
                else
                {
                    user.VerificationDocument.IsVerified = true;
                    user.AuthProfile.IsVerified = true;
                    user.ModifiedAt = DateTimeOffset.UtcNow;
                    user.VerificationStatus = VerificationStatus.Approved;

                    await dbContext.SaveChangesAsync();
                    logger.LogInformation($"User verification status updated successfully for {name} with ID {user.Id}.");
                    await notificationHub.Clients.User(user.Id.ToString()).SendAsync("KycSuccessful", new
                    {
                        message = $"Your KYC was verified successful",
                        time = DateTime.UtcNow
                    });
                    return ResponseDetail<bool>.Successful(true, $"User verification status updated successfully for {name}.");
                }
            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, $"updating user verification status for {name}");
                return ResponseDetail<bool>.Failed(false, $"An error occurred while updating user verification status for {name}. Please try again later.");
            }
        }
    }
}
