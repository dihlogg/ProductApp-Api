using WavesOfFoodDemo.Server.Dtos;

namespace WavesOfFoodDemo.Server.Services;

public interface ICartDetailsService
{
    Task<List<CartDetailsDto>> GetCartDetailsDtosAsync();
    Task<bool> AddCartDetailsAsync(CartDetailsCreateDto cartDetailsCreateDto);
    Task<bool?> EditCartDetailsAsync(CartDetailsDto cartDetailsDto);
    Task<bool?> RemoveCartDetailsDtosAsync(Guid id);
    Task<bool> ProcessOrderAsync(Guid userId, List<CartDetailsRequestCreateDto> products);
}

