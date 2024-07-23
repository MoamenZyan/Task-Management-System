using Project.API.Interfaces;
using StackExchange.Redis;

namespace Project.API.Services
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        public CacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public async Task Del(string key)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key);
        }

        public async Task<string?> Get(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task Set(string key, string value)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value, TimeSpan.FromMinutes(5));
        }
    }
}
