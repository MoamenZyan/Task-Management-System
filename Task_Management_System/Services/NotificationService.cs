using Project.API.Interfaces;

namespace Project.API.Services
{
    public class NotificationService : INotificationService
    {
        private INotificationStrategy _notificationStrategy = null!;
        public void SetNotificationStrategy(INotificationStrategy strategy)
        {
            _notificationStrategy = strategy;
        }

        public async Task Send(string to, string userName, Dictionary<string, string> body)
        {
           await _notificationStrategy.Send(to, userName, body);
        }
    }
}
