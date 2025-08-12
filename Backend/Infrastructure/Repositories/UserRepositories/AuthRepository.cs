using Hangfire;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.BackgroudServices;
using Services.ExternalAPI_Integration;
using Services.MailingService;
using SharedModule.Settings;
using SharedModule.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserModule.DTOs.AuthDTOs;
using UserModule.Interfaces.UserInterfaces;

namespace Infrastructure.Repositories.UserRepositories
{
    public class AuthRepository : IAuthService
    {
        private readonly ILogger<AuthRepository> logger;
        private readonly IMailService mailer;
        private readonly IMemoryCache cache;
        private readonly AppSettings settings;
        private readonly Secrets secrets;
        private readonly LogHelper<AuthRepository> logHelper;
        private readonly BaraContext dbContext;
        private readonly HangfireJobs hangfire;
        private readonly ExternalApiIntegrationService externalService;
        public AuthRepository(BaraContext baraContext, IOptions<Secrets> secrets, IOptions<AppSettings> appSettings,
            ILogger<AuthRepository> logger, IMemoryCache memoryCache, IMailService mailService, LogHelper<AuthRepository> logHelper, HangfireJobs hangfire, ExternalApiIntegrationService externalApiIntegrationService)
        {
            this.secrets = secrets.Value;
            mailer = mailService;
            cache = memoryCache;
            this.logger = logger;
            settings = appSettings.Value;
            this.logHelper = logHelper;
            dbContext = baraContext;
            this.hangfire = hangfire;
            externalService = externalApiIntegrationService;
        }
        public async Task<ResponseDetail<LoginResponseDTO>> Login(AuthRequestDTO authReqBody)
        {
            var email = authReqBody.Email.ToLower();
            try
            {
                var validationErrors = new List<string>();
                var user = await dbContext.AuthProfiles.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return ResponseDetail<LoginResponseDTO>.Failed("Login unsuccessful...Email or password is invalid");
                }
                var response = new LoginResponseDTO
                {
                    Email = authReqBody.Email,
                    Name = user.FullName,
                    UserId = user.UserId,
                };
                var confirmPassword = BCrypt.Net.BCrypt.Verify(authReqBody.Password, user.Password);
                if (!confirmPassword)
                {
                    user.LoginAttempts += 1;
                    user.ModifiedAt = DateTimeOffset.UtcNow;
                    response.WrongLoginAttempts = user.LoginAttempts;
                    dbContext.AuthProfiles.Update(user);
                    await dbContext.SaveChangesAsync();
                    return ResponseDetail<LoginResponseDTO>.Failed(response, "Login unsuccessful...Email or password is invalid");
                }
                else if (user.IsLocked || user.LoginAttempts > 5)
                {
                    response.WrongLoginAttempts = user.LoginAttempts;
                    Expression<Func<AuthRepository, Task>> job = s => s.UnlockAccount(user.UserId);
                    BackgroundJob.Schedule(job, TimeSpan.FromHours(1));

                    return ResponseDetail<LoginResponseDTO>.Failed(response, "Login unsuccessful...account has been blocked due to too many wrong email or password attempts... Try again in 1hr");
                }

                else if (user.IsDeleted == true) validationErrors.Add("Login unsuccessful... user account has been deactivated and needs to be reactivated");
                else if (!user.IsEmailVerified) validationErrors.Add("Login unsuccessful...Email address is unverified... Please verify yout email and try again");
                else if (!user.IsVerified) validationErrors.Add("Login unsuccessful... Account verification failed or is still in progress");

                if (validationErrors.Count > 0)
                {
                    return ResponseDetail<LoginResponseDTO>.Failed(string.Join(" |\n ", validationErrors), 403, "Forbidden");
                }

                var (Ip, Country) = await externalService.GetIpAndCountryAsync(secrets.IpInfoKey);
                if (user.LastLoginDevice != authReqBody.LoginDevice || user.LastLoginIPAddress != Ip)
                {
                    var cacheKey = $"User_Login_Token_{user.UserId}";
                    var token = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
                    cache.Set(cacheKey, token.ToString(), absoluteExpiration: DateTimeOffset.UtcNow.AddMinutes(10));
                    logger.LogInformation($"User_Login_Token_{user.UserId}: {token}");
                    Console.WriteLine($"User_Login_Token_{user.UserId}: {token}");

                    var mailBody = MailNotifications.LoginNotification(email, user.FullName, token, authReqBody.LoginDevice, Ip, Country);
                    var mailRes = await mailer.SendMail(mailBody);
                    if (!mailRes.IsSuccess)
                    {
                        return ResponseDetail<LoginResponseDTO>.Failed($"An error occured while sending a login notification mail", 500, "Unexpected Error");
                    }
                    if (user.LoginAttempts > 0)
                    {
                        response.WrongLoginAttempts = user.LoginAttempts;
                        user.LoginAttempts = 0;
                        user.ModifiedAt = DateTimeOffset.UtcNow;
                        dbContext.AuthProfiles.Update(user);
                        await dbContext.SaveChangesAsync();
                    }
                    return ResponseDetail<LoginResponseDTO>.Successful(response, "Please verify login attempt");
                }
                else
                {
                    var accessToken = GenerateJwtToken(user.Role, user.IsVerified ? "Verified" : "Unverified", user.UserId);

                    response.AccessToken = accessToken;
                    response.WrongLoginAttempts = user.LoginAttempts;
                    user.LoginAttempts = 0;
                    user.LastLoginAt = DateTimeOffset.UtcNow;
                    user.ModifiedAt = DateTimeOffset.UtcNow;

                    dbContext.AuthProfiles.Update(user);
                    await dbContext.SaveChangesAsync();
                    return ResponseDetail<LoginResponseDTO>.Successful(response, "Login Successful");
                }
            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, $"attempting to login with {authReqBody.Email}", ex.Message);
                return ResponseDetail<LoginResponseDTO>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }
        public Task<ResponseDetail<bool>> ChangePassword(PasswordChangeDTO reqBody)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateRefreshToken(string token)
        {
            throw new NotImplementedException();
        }


        public async Task Logout(Guid userId)
        {
            try
            {
                var user = await dbContext.AuthProfiles.FindAsync(userId);
                if (user == null)
                {
                    logger.LogWarning($"User with ID {userId} not found during logout attempt.");
                    return;
                }
                user.LastLogoutAt = DateTimeOffset.UtcNow;
                user.ModifiedAt = DateTimeOffset.UtcNow;
                dbContext.AuthProfiles.Update(user);
                await dbContext.SaveChangesAsync();

                logger.LogInformation($"User: {user.FullName} has been logged out successfully.");

                var cacheKey = $"User_Login_Token_{user.UserId}";
                if (cache.TryGetValue(cacheKey, out string verificationToken))
                {
                    cache.Remove(cacheKey);
                    logger.LogInformation("Removed login token for user: {name} with ID: {userId}", user.FullName, user.UserId);
                    return;
                }
                else
                {
                    logger.LogWarning($"No login token found for user: {user.FullName} with ID: {user.UserId}");
                    return;
                }
            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, $"Logging out user with ID {userId}", ex.Message);
            }
        }

        public async Task<ResponseDetail<string>> ResendVerificationToken(string email)
        {
            try
            {
                var user = await dbContext.AuthProfiles.Select(x => new { x.Email, x.UserId }).FirstOrDefaultAsync(x => x.Email == email);
                var token = RandomNumberGenerator.GetInt32(100000, 999999);
                if (user == null)
                {
                    return ResponseDetail<string>.Failed("Account doens't exist", 404, "Not Found");
                }
                else
                {
                    cache.Set($"User_Verification_Token_{user.UserId}", token.ToString(), absoluteExpiration: DateTimeOffset.UtcNow.AddMinutes(10));
                    logger.LogInformation($"User_Verification_Token_{user.Email}: {token}");
                    Console.WriteLine($"User_Verification_Token_{user.Email}: {token}");
                    var verificationMail = MailNotifications.RegistrationConfirmationMailNotification(user.Email, "", token.ToString());
                    var mailRes = await mailer.SendMail(verificationMail);
                    if (mailRes.IsSuccess == false)
                    {
                        return ResponseDetail<string>.Failed($"An error occured while resending verification mail", 500, "Unexpected Error");
                    }
                }
                return ResponseDetail<string>.Successful($"Verification token: {token} has been successfully sent");
            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, $"Verifying Email {email}", ex.Message);
                return ResponseDetail<string>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public async Task<ResponseDetail<bool>> VerifyEmail(string token, string email)
        {
            try
            {
                var user = await dbContext.AuthProfiles.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    return ResponseDetail<bool>.Failed($"Operation can not be completed because user does not exist", 404);
                }
                else if (user.IsEmailVerified)
                {
                    return ResponseDetail<bool>.Failed("User email is already verified.", 409);
                }
                else
                {
                    var cacheKey = $"User_Verification_Token_{user.UserId}";
                    cache.TryGetValue(cacheKey, out string verificationToken);
                    if (verificationToken == null || token != verificationToken)
                    {
                        return ResponseDetail<bool>.Failed("Operation failed because of invalid or expired token... Please try again");
                    }
                    user.IsEmailVerified = true;
                    user.ModifiedAt = DateTimeOffset.UtcNow;
                    cache.Remove(cacheKey);

                    dbContext.AuthProfiles.Update(user);
                    await dbContext.SaveChangesAsync();
                    return ResponseDetail<bool>.Successful(true, "Email verified successfully");
                }
            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, $"Verifying Email for {email}", ex.Message);
                return ResponseDetail<bool>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        public async Task<ResponseDetail<LoginResponseDTO>> VerifyLogin(LoginVerificationDTO loginDetails)
        {
            try
            {
                var user = await dbContext.AuthProfiles.FirstOrDefaultAsync(x => x.Email == loginDetails.Email);
                if (user == null)
                {
                    return ResponseDetail<LoginResponseDTO>.Failed($"{loginDetails.Email} is invalid or doesn't exist", 400, "Bad Request");
                }
                var response = new LoginResponseDTO
                {
                    Email = loginDetails.Email,
                    Name = user.FullName,
                    UserId = user.Id,
                    WrongLoginAttempts = user.LoginAttempts,
                };

                var cacheKey = $"User_Login_Token_{user.UserId}";
                cache.TryGetValue(cacheKey, out string verificationToken);
                if (verificationToken == null || loginDetails.Token != verificationToken)
                {
                    return ResponseDetail<LoginResponseDTO>.Failed("Operation can't be completed at the moment because the token is invalid or expired", 403, "Forbidden");
                }
                cache.Remove(cacheKey);
                var jwt_token = GenerateJwtToken(user.Role, user.IsVerified ? "Verified" : "Unverified", user.UserId);
                var (Ip, Country) = await externalService.GetIpAndCountryAsync(secrets.IpInfoKey);

                response.AccessToken = jwt_token;
                response.WrongLoginAttempts = 0;
                user.LoginAttempts = 0;
                user.LastLoginDevice = loginDetails.Device;
                user.LastLoginIPAddress = Ip;
                user.LastLoginAt = DateTimeOffset.UtcNow;
                user.ModifiedAt = DateTimeOffset.UtcNow;

                dbContext.AuthProfiles.Update(user);
                await dbContext.SaveChangesAsync();
                return ResponseDetail<LoginResponseDTO>.Successful(response, "Login Successful");
            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, $"Verifying Email: {loginDetails.Email} for login", ex.Message);
                return ResponseDetail<LoginResponseDTO>.Failed("Your request cannot be completed at this time... Please try again later", 500, "Unexpected error");
            }
        }

        // Here lies UnlockAccount method: private.
        // Called only by this class.
        // Let no other dare invoke or try to perform any magic, lest the compiler raises its voice... Well, except you can hangle the errors you get (if any).
        //You have been warned
        [AutomaticRetry(Attempts = 3, DelaysInSeconds = [10, 30, 60])]
        private async Task<bool> UnlockAccount(Guid userId)
        {
            string name = "";
            try
            {
                var user = await dbContext.AuthProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
                if (user != null)
                {
                    name = user.FullName;
                    user.IsLocked = false;
                    user.LoginAttempts = 0;
                    dbContext.AuthProfiles.Update(user);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logHelper.LogExceptionError(ex.GetType().Name, ex.GetBaseException().GetType().Name, $"unlocking user {name}'s account ", ex.Message);
                return false;
            }
        }

        private string GenerateJwtToken(string role, string verificationStatus, Guid userId)
        {
            var claims = new List<Claim>
            {
                new("UserId", userId.ToString()),
                new ("Role", role),
                new("VerificationStatus", verificationStatus)
            };
            var random = new Random();
            string issuer = secrets.Issuers[random.Next(secrets.Issuers.Length)];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrets.JwtSickRit));
            var signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var token = new JwtSecurityToken(
                issuer,
                signingCredentials: signingCred,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60)
                );
            //SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Issuer = settings.Issuer,
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddMinutes(60),
            //    SigningCredentials = signingCred
            //};

            var finalToken = new JwtSecurityTokenHandler().WriteToken(token);
            return finalToken;
        }
    }
}
