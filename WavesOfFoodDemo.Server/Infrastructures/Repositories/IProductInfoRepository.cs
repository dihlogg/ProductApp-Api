using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Dtos.Clustering;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public interface IProductInfoRepository : IGenericRepository<ProductInfo>
{
    Task<List<ProductInfo>> GetPopularProducts();
    Task<List<ProductInfo>> SearchProductInfoDtosAsync(string productName);
    Task<bool> AddProductAsync(ProductInfo productInfo);
    Task<List<ProductInfoDto>> GetProductAsync();
    Task<bool> UpdateProductImagesAsync(ProductInfo productInfo, List<ProductImageDto> newImages);
    Task<ProductInfoDto?> GetProductDetailsByIdAsync(Guid id);
    Task<List<ProductInfoDto>> GetProductsByCategoryIdAsync(Guid categoryId);
    Task UpdateProductAsync(ProductInfo productInfo);
    Task<List<ProductFeatureDto>> GetProductFeaturesAsync();
}