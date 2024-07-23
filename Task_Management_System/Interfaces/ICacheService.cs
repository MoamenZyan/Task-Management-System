namespace Project.API.Interfaces
{
    public interface ICacheService
    {
        Task<string?> Get(string key);
        Task Set(string key, string value);
        Task Del(string key);
    }
}
