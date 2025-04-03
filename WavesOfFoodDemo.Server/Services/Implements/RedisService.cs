using StackExchange.Redis;

namespace WavesOfFoodDemo.Server.Services.Implements
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _defaultDatabase; // Redis 6379
        private readonly IDatabase _secondaryDatabase; // Redis 6380
        private readonly IConnectionMultiplexer _defaultRedis;
        private readonly IConnectionMultiplexer _secondaryRedis;

        public RedisService(
            IConnectionMultiplexer defaultRedis, // Instance 6379
            IConnectionMultiplexer secondaryRedis) // Instance 6380
        {
            _defaultDatabase = defaultRedis.GetDatabase();
            _secondaryDatabase = secondaryRedis.GetDatabase();
            _defaultRedis = defaultRedis;
            _secondaryRedis = secondaryRedis;
        }

        public async Task SetValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            await _defaultDatabase.StringSetAsync(key, value, expiry);
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _defaultDatabase.StringGetAsync(key);
        }
        public async Task<bool> DeleteKeyAsync(string key)
        {
            var db = _defaultDatabase;
            return await db.KeyDeleteAsync(key);
        }
        public async Task<IEnumerable<string>> GetAllKeysAsync(string pattern)
        {
            var server = _defaultRedis.GetServer(_defaultRedis.GetEndPoints().First());
            return server.Keys(pattern: pattern).Select(k => k.ToString());
        }
        public async Task SetClusterDataAsync(Guid productId, int clusterId)
        {
            await _secondaryDatabase.StringSetAsync($"Cluster:{productId}", clusterId);
        }

        public async Task<int?> GetClusterDataAsync(Guid productId)
        {
            var clusterId = await _secondaryDatabase.StringGetAsync($"Cluster:{productId}");
            return clusterId.HasValue ? (int)clusterId : null;
        }
    }
}
