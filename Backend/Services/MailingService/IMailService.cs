using SharedModule.Utils;

namespace Services.MailingService
{
    public interface IMailService
    {
        Task<ResponseDetail<string>> SendMail(MailRequestDTO mail);
    }
}
