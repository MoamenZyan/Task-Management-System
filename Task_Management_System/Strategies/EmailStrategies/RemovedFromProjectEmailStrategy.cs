using Project.API.Interfaces;
using Project.API.Utils;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace Project.API.Strategies.EmailStrategies
{
    public class RemovedFromProjectEmailStrategy(SendGridSettings sendGridSettings) : INotificationStrategy
    {
        public async Task Send(string to, string userName, Dictionary<string, string> body)
        {
            try
            {
                var client = new SendGridClient(sendGridSettings.APIKey);
                var from = sendGridSettings.FromEmail;
                var toEmail = new EmailAddress(to);
                var htmlContent = $"<h1>{userName}, You have been removed from project {body["ProjectName"]}</h1>";
                var subject = "Removed From A Project !";
                var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, "", htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }
    }
}
