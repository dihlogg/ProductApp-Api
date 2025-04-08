using WavesOfFoodDemo.Server.Dtos;

namespace WavesOfFoodDemo.Server.Services;

public interface IProductInfoHistoryService
{
    Task GenerateDailyTopSellingProducts();
    Task<List<ProductInfoHistoryDto>> GetRandomTop10Products();
}
