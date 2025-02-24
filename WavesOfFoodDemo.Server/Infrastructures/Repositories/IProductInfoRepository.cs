using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public interface IProductInfoRepository : IGenericRepository<ProductInfo>
{
    Task<List<ProductInfoDto>> GetPopularProducts();
    Task<List<ProductInfo>> SearchProductInfoDtosAsync(string productName);
}