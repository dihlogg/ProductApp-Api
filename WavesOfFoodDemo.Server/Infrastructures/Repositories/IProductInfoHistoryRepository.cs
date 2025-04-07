using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures.Repositories;

public interface IProductInfoHistoryRepository : IGenericRepository<ProductInfoHistory>
{
    Task<List<(Guid ProductId, int QuantitySold)>> GetTopSellingProductsYesterday();
    Task SaveProductInfoHistory(List<ProductInfoHistory> histories);
    Task<List<ProductInfo>> GetProductInfosByIds(IEnumerable<Guid> productIds);
    Task DeleteAllProductInfoHistory();
    Task<List<ProductInfoHistoryDto>> GetProductInfoHistories();
}
