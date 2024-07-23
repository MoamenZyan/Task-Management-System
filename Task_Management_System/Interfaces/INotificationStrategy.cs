namespace Project.API.Interfaces
{
    public interface INotificationStrategy
    {
        Task Send(string to, string userName, Dictionary<string, string> body);
    }
}
