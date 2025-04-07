namespace WavesOfFoodDemo.Server.Services
{
    public interface IRedisService
    {
        Task SetValueAsync(string key, string value, TimeSpan? expiry = null);
        Task<string> GetValueAsync(string key);
        Task<bool> DeleteKeyAsync(string key);
        Task<IEnumerable<string>> GetAllKeysAsync(string pattern);
        Task SetClusterDataAsync(Guid categoryId, Guid productId, int clusterId);
        Task<int?> GetClusterDataAsync(Guid categoryId, Guid productId);
    }
}
