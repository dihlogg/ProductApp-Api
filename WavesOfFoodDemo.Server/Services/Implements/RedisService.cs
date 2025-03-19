using System.Text.Json;
using StackExchange.Redis;

namespace WavesOfFoodDemo.Server.Services.Implements
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _redis;

        public RedisService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
            _redis = redis;
        }

        public async Task SetValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            await _database.StringSetAsync(key, value, expiry);
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }
        public async Task<bool> DeleteKeyAsync(string key)
        {
            var db = _database;
            return await db.KeyDeleteAsync(key);
        }
        public async Task<IEnumerable<string>> GetAllKeysAsync(string pattern)
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            return server.Keys(pattern: pattern).Select(k => k.ToString());
        }
    }
}
