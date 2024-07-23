namespace Project.API.Interfaces
{
    public interface INotificationService
    {
        void SetNotificationStrategy(INotificationStrategy strategy);
        Task Send(string to, string userName, Dictionary<string, string> body);
    }
}
