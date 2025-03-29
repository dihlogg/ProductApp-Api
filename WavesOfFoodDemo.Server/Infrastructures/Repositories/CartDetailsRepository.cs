using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public class CartDetailsRepository : GenericRepository<CartDetails>, ICartDetailsRepository
{
    public CartDetailsRepository(ProductDbContext productDbContext) : base(productDbContext)
    {
    }
    public async Task<ProductInfo> GetProductByIdAsync(Guid productId)
    {
        return await _productDbContext.ProductInfos.FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task UpdateProductAsync(ProductInfo productInfo)
    {
        _productDbContext.ProductInfos.Update(productInfo);
        await _productDbContext.SaveChangesAsync();
    }
}