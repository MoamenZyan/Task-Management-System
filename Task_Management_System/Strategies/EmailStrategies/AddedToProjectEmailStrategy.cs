using Project.API.Utils;
using Project.API.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace Project.API.Strategies.EmailStrategies
{
    public class AddedToProjectEmailStrategy(SendGridSettings sendGridSettings) : INotificationStrategy
    {
        public async Task Send(string to, string userName, Dictionary<string, string> body)
        {
            try
            {
                var client = new SendGridClient(sendGridSettings.APIKey);
                var from = sendGridSettings.FromEmail;
                var toEmail = new EmailAddress(to);
                var htmlContent = $"<h1>{userName}, You have been added to project {body["ProjectName"]} as {body["UserRole"]}</h1>";
                var subject = "Added to project !";
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
