using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public interface IProductInfoRepository : IGenericRepository<ProductInfo>
{
    Task<List<ProductInfoDto>> GetPopularProducts();
    Task<List<ProductInfo>> SearchProductInfoDtosAsync(string productName);
    Task<bool> AddProductAsync(ProductInfo productInfo);
    Task<List<ProductInfo>> GetProductAsync();
    Task<bool> UpdateProductImagesAsync(ProductInfo productInfo, List<ProductImageDto> newImages);
}