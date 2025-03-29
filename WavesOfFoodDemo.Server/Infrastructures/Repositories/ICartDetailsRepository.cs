using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public interface ICartDetailsRepository : IGenericRepository<CartDetails>
{
    Task<ProductInfo> GetProductByIdAsync(Guid productId);
    Task UpdateProductAsync(ProductInfo productInfo);
}
