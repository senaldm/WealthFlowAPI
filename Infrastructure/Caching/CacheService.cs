using StackExchange.Redis;
using WealthFlow.Application.Caching.Interfaces;

namespace WealthFlow.Infrastructure.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cache;

        public CacheService(IConfiguration configuaration)
        {
            var redis = ConnectionMultiplexer.Connect(configuaration["Redis:ConnectionString"]);
            _cache = redis.GetDatabase();
        }

        public async Task<string?> GetAsync(string key)
        {
            return await _cache.StringGetAsync(key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
             return await _cache.KeyDeleteAsync(key);
        }

        public async Task<bool> StoreAsync(string key, string value, TimeSpan expiration)
        {
            return await _cache.StringSetAsync(key, value, expiration);
        }
    }
}
