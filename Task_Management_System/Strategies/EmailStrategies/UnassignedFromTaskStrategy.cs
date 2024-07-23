using Project.API.Interfaces;
using Project.API.Utils;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Project.API.Strategies.EmailStrategies
{
    public class UnassignedFromTaskEmailStrategy(SendGridSettings sendGridSettings) : INotificationStrategy
    {
        public async Task Send(string to, string userName, Dictionary<string, string> body)
        {
            try
            {
                var client = new SendGridClient(sendGridSettings.APIKey);
                var from = sendGridSettings.FromEmail;
                var toEmail = new EmailAddress(to);
                var htmlContent = $"<h1>{userName}, You have been unassigned from task {body["TodoName"]}</h1>";
                var subject = "Unassigned from task !";
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
