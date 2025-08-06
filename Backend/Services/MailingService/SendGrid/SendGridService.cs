using SharedModule.Utils;

namespace Services.MailingService.SendGrid
{
    public class SendGridService : IMailService
    {
        public Task<ResponseDetail<bool>> SendMail(MailRequestDTO mail)
        {
            throw new NotImplementedException();
        }
    }
}
