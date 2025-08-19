using Hangfire;
using Infrastructure.DataContext;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Services.BackgroudServices;
using Services.MailingService;
using Services.SignalR;
using SharedModule.Utils;
using System.Security.Cryptography;
using UserModule.DTOs.UserDTO;
using UserModule.Enums;
using UserModule.Interfaces.UserInterfaces;
using UserModule.Models;
using UserModule.Utilities;

namespace Infrastructure.Repositories.UserRepositories
{
    public class UserRepository : IUserService
    {
        private readonly BaraContext dbContext;
        private readonly LogHelper<UserRepository> logHelper;
        private readonly ILogger<UserRepository> logger;
        private readonly IHubContext<NotificationHub> notificationHub;
        private readonly HangfireJobs hangfire;
        private readonly IMemoryCache cache;
        public UserRepository(BaraContext baraContext, LogHelper<UserRepository> logHelper, HangfireJobs hangfire,
        ILogger<UserRepository> logger, IHubContext<NotificationHub> hubContext, IMemoryCache cache)
        {
            dbContext = baraContext;
            this.logHelper = logHelper;
            this.logger = logger;
            notificationHub = hubContext;
            this.hangfire = hangfire;
            this.cache = cache;
        }

        public async Task<ResponseDetail<RegisterResponseDTO>> BeginRegistration(RegisterDTO detail)
        {
            try
            {
                var validationErrors = new List<string>();
                var userProfile = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == detail.Email);
                if (userProfile is not null)
                {
                    validationErrors.Add($"A user with the email {detail.Email} already exists.");
                }
                else if (userProfile?.IsDeleted == false)
                {
                    validationErrors.Add($"Please contact support to restore your account.");
                    return ResponseDetail<RegisterResponseDTO>.Failed(string.Join(" ; ", validationErrors), 409, "Conflict");
                }

                var emailValidation = RegexValidations.IsValidMail(detail.Email);
                var passwordValidation = RegexValidations.IsAcceptablePasswordFormat(detail.Password);

                if (!emailValidation || !passwordValidation)
                {
                    if (!emailValidation) validationErrors.Add("Invalid email format");
                    if (!passwordValidation) validationErrors.Add("Password must be strong (at least 8 characters, one uppercase, one lowercase, one number, and one special character)");
                    return ResponseDetail<RegisterResponseDTO>.Failed(string.Join(" | ", validationErrors), 400);
                }

                // --------------------  GENERATE EMAIL VERIFICATION TOKEN --------------------
                var token = RandomNumberGenerator.GetInt32(100000, 999999);

                cache.Set($"User_Verification_Token_{detail.Email}", token.ToString(), absoluteExpiration: DateTimeOffset.UtcNow.AddMinutes(10));
                Console.WriteLine($"Writer_Verification_Token_{detail.Email}: {token}");
                logger.LogInformation($"Writer_Verification_Token_{detail.Email}: {token}");

                var verificationMail = MailNotifications.RegistrationConfirmationMailNotification(detail.Email, token.ToString());


                var user = new User
                {
                    Email = detail.Email,
                    Type = detail.Type,
                    VerificationStatus = detail.Type == Role.Admin ? VerificationStatus.Approved : VerificationStatus.Pending,
                    AuthProfile = new AuthProfile
                    {
                        Email = detail.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(detail.Password),
                        Role = detail.Type.ToString(),
                        IsVerified = detail.Type == Role.Admin
                    },
                };
                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                BackgroundJob.Enqueue(() => hangfire.SendMailAsync(verificationMail));
                var response = new RegisterResponseDTO
                {
                    UserId = user.Id,
                    Email = user.Email,
                };
                return ResponseDetail<RegisterResponseDTO>.Successful(response, $"A verification token has been sent to {detail.Email}. Please check your inbox to complete the registration process.", 201);
            }
            catch (Exception ex)
            {
                logger.LogError($"An exception: {ex.GetType().Name} was thrown while creating a writer profile... \nBase Exception: {ex.GetBaseException().GetType().Name}", $"Exception Code: {ex.HResult}", ex.Message);
                return ResponseDetail<RegisterResponseDTO>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
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

        public async Task<ResponseDetail<bool>> UpdateUserVerificationStatus(string verificationIdNumber, string dateOfBirth, string firstName, string lastName, string type)
        {
            string name = "";
            try
            {
                var user = await dbContext.Users
                    .Include(x => x.Document)
                    .Include(x => x.AuthProfile)
                    .FirstOrDefaultAsync(x => x.Document.IdentificationNumber == verificationIdNumber);

                var errors = new List<string>();

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

                    logger.LogInformation($"Verification for {user.FirstName} {user.LastName} failed because {string.Join(" |\n ", errors)}");
                    return ResponseDetail<bool>.Failed(false, string.Join(" |\n ", errors));
                }
                else
                {
                    user.Document.IsVerified = true;
                    user.AuthProfile.IsVerified = true;
                    user.ModifiedAt = DateTimeOffset.UtcNow;
                    user.VerificationStatus = VerificationStatus.Approved;
                    user.Document.IsVerified = true;
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
