using WavesOfFoodDemo.Server.Dtos;

namespace WavesOfFoodDemo.Server.Services
{
    public interface IProductImageService
    {
        Task<List<ProductImageDto>> GetProductImageDtosAsync();
        Task<bool> AddProductImageAsync(ProductImageCreateDto productImageCreateDto);
        Task<bool?> EditProductImageAsync(ProductImageDto productImageDto);
        Task<bool?> RemoveProductImageDtosAsync(Guid id);
    }
}
