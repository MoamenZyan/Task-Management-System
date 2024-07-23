using Project.API.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;
using Project.API.Utils;

namespace Project.API.Strategies.EmailStrategies
{
    public class AssignedFromTaskEmailStrategy(SendGridSettings sendGridSettings) : INotificationStrategy
    {
        public async Task Send(string to, string userName, Dictionary<string, string> body)
        {
            try
            {
                var client = new SendGridClient(sendGridSettings.APIKey);
                var from = sendGridSettings.FromEmail;
                var toEmail = new EmailAddress(to);
                var htmlContent = $"<h1>{userName}, You have been assigned to a task {body["TodoName"]} and it's deadline: {body["Deadline"]}</h1>";
                var subject = "Assigned to a task !";
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
