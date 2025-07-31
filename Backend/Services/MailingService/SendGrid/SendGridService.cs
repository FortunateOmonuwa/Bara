using SharedModule.Utils;

namespace Services.MailingService.SendGrid
{
    public class SendGridService : IMailService
    {
        public Task<ResponseDetail<string>> SendMail(MailRequestDTO mail)
        {
            throw new NotImplementedException();
        }
    }
}
