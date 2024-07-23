using SendGrid;
using SendGrid.Helpers.Mail;
namespace Project.API.Utils
{
    public class SendGridSettings
    {
        public string APIKey { get; set; } = null!;
        public EmailAddress FromEmail { get; set; } = new EmailAddress("blink.blog.website@gmail.com", "Task Management System");
    }
}
