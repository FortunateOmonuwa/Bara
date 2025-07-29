using Microsoft.AspNetCore.Http;

namespace Services.MailingService
{
    public class MailRequestDTO
    {
        public required string Receiver { get; set; }
        public string Subject { get; set; } = "Bara Notification";
        public required string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
