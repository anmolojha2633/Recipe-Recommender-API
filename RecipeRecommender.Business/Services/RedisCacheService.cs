using StackExchange.Redis;
using System.Text.Json;

namespace RecipeRecommender.Business.Services
{
    public class RedisCacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, int expiryMinutes = 10)
        {
            var json = JsonSerializer.Serialize(value);

            await _db.StringSetAsync(
                key,
                json,
                TimeSpan.FromMinutes(expiryMinutes)
            );
        }
    }
}