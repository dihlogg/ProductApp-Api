using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public class ProductInfoRepository : GenericRepository<ProductInfo>, IProductInfoRepository
{
    public ProductInfoRepository(ProductDbContext productDbContext) : base(productDbContext)
    {
    }
  
    public async Task<List<ProductInfo>> SearchProductInfoDtosAsync(string productName)
    {
        var query = _productDbContext.ProductInfos.AsQueryable();
        query = query.Where(s => s.Name.Contains(productName));
        return await query.AsNoTracking().ToListAsync();
    }

    public Task<List<ProductInfoDto>> GetPopularProducts()
    {
        throw new NotImplementedException();
    }
}