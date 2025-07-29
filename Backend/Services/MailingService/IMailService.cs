using SharedModule.Utils;

namespace Services.MailingService
{
    public interface IMailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mail"></param>
        /// <returns>A response detail of type string that specifies if the mail was sent or not</returns>
        Task<ResponseDetail<string>> SendMail(MailRequestDTO mail);
    }
}
