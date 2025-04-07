using WavesOfFoodDemo.Server.Dtos.Clustering;

namespace WavesOfFoodDemo.Server.Services
{
    public interface IMLService
    {
        Task<List<ProductFeatureDto>> GetSimilarProductsAsync(Guid productId);
        Task UpdateClustersAsync();
    }
}
