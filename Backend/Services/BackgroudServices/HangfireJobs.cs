using Hangfire;
using Microsoft.Extensions.Logging;
using Services.MailingService;
using IMailService = Services.MailingService.IMailService;

namespace Services.BackgroudServices
{
    /// <summary>
    /// This class is responsible for managing background jobs using Hangfire.
    /// </summary>
    public class HangfireJobs
    {
        private readonly ILogger<HangfireJobs> logger;
        private readonly IMailService mailService;
        public HangfireJobs(IMailService mailService, ILogger<HangfireJobs> logger)
        {
            this.mailService = mailService;
            this.logger = logger;
        }

        [AutomaticRetry(Attempts = 3, DelaysInSeconds = [10, 30, 60])]
        public async Task SendMailAsync(MailRequestDTO mail)
        {
            try
            {
                var response = await mailService.SendMail(mail);
                if (response.IsSuccess)
                {
                    logger.LogInformation($"Email sent successfully to {mail.Receiver}");
                }
                else
                {
                    logger.LogError($"Failed to send email to {mail.Receiver}: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while sending email to {mail.Receiver}");
            }
        }
    }
}
