using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Dtos.CartDetails;

namespace WavesOfFoodDemo.Server.Services;

public interface ICartInfoService
{
    Task<List<CartInfoDto>> GetCartInfoDtosAsync();
    Task<bool> AddCartInfoAsync(CartInfoCreateDto cartInfoCreateDto);
    Task<bool?> EditCartInfoAsync(CartInfoDto cartInfoDto);
    Task<bool?> RemoveCartInfoDtosAsync(Guid id);
    Task<IEnumerable<CartHistoryDto>> GetTransactions(Guid userId);
    Task<bool?> PostCartDetailInfo(CartDetailInfoDto cartInfoCreateDto);
}

