using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SharedModule.Settings;
using SharedModule.Utils;

namespace Services.MailingService.SMTP
{
    public class SMTP_Service(IOptions<AppSettings> appSettings, ILogger<SMTP_Service> logger) : IMailService
    {
        private readonly AppSettings settings = appSettings.Value;
        private readonly ILogger<SMTP_Service> logger = logger;

        public async Task<ResponseDetail<bool>> SendMail(MailRequestDTO mail)
        {
            try
            {
                var message = new MimeMessage
                {
                    To = { MailboxAddress.Parse(mail.Receiver) },
                    From = { MailboxAddress.Parse(settings.Sender) },
                    Subject = mail.Subject
                };


                var textPart = new TextPart(TextFormat.Html)
                {
                    Text = mail.Body
                };

                var multipart = new Multipart("mixed")
                {
                    textPart
                };

                if (mail.Attachments != null && mail.Attachments.Count != 0)
                {
                    foreach (var file in mail.Attachments)
                    {
                        using var ms = new MemoryStream();
                        await file.CopyToAsync(ms);
                        ms.Position = 0;

                        var attachment = new MimePart()
                        {
                            Content = new MimeContent(ms),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = file.FileName
                        };

                        multipart.Add(attachment);
                    }
                }

                message.Body = multipart;

                using var client = new SmtpClient();
                client.Connect(settings.Server, settings.Port, SecureSocketOptions.StartTls);
                client.Authenticate(settings.Sender, settings.Password);

                await client.SendAsync(message);
                client.Disconnect(true);

                logger.LogInformation($"Mail to {mail.Receiver} was successfully sent");
                return ResponseDetail<bool>.Successful(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An exception occurred while sending mail to {mail?.Receiver}");
                return ResponseDetail<bool>.Failed(ex.Message, ex.HResult, "Caught Exception");
            }
        }
    }
}
