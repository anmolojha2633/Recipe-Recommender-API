using StackExchange.Redis;
using System.Text.Json;

namespace RecipeRecommender.Business.Services
{
    public class RedisCacheService
    {
        private readonly IDatabase? _db;

        public RedisCacheService(IConnectionMultiplexer? redis)
        {
            _db = redis?.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (_db == null)
                return default;

            try
            {
                var value = await _db.StringGetAsync(key);

                if (value.IsNullOrEmpty)
                    return default;

                return JsonSerializer.Deserialize<T>(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving from Redis: {ex.Message}");
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, int expiryMinutes = 10)
        {
            if (_db == null) return;

            try
            {
                var json = JsonSerializer.Serialize(value);

                await _db.StringSetAsync(
                    key,
                    json,
                    TimeSpan.FromMinutes(expiryMinutes)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting Redis cache: {ex.Message}");
            }
        }
    }
}


