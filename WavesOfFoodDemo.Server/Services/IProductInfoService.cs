using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Dtos.Clustering;

namespace WavesOfFoodDemo.Server.Services;

public interface IProductInfoService
{
    Task<List<ProductInfoDto>> GetProductInfoDtosAsync();
    Task<bool> AddProductInfoAsync(ProductInfoCreateDto productInfoCreateDto);
    Task<bool?> EditProductInfoAsync(ProductInfoDto productInfoDto);
    Task<bool?> RemoveProductInfoDtosAsync(Guid id);
    Task<List<ProductInfoDto>> SearchProductInfoDtosAsync(string productName);
    Task<List<ProductInfoDto>> GetPopularProducts();
    Task<ProductInfoDto?> GetProductDetailsById(Guid id);
    Task<List<ProductInfoDto>> GetProductsByCategoryIdAsync(Guid categoryId);
    Task<List<ProductFeatureDto>> GetProductFeaturesAsync();
}
