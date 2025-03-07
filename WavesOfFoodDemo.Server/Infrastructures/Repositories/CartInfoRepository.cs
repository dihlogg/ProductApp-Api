using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public class CartInfoRepository : GenericRepository<CartInfo>, ICartInfoRepository
{
    public CartInfoRepository(ProductDbContext productDbContext) : base(productDbContext)
    {
    }
   
    public async Task<IEnumerable<CartHistoryDto>> GetTransactions(Guid userId)
    {
        var query = _productDbContext.CartInfos
            .Where(item => item.UserId == userId)
            .AsNoTracking()
            .AsQueryable();
        //foreach (var cartInfo in query)
        //{
        //    var newObject = new CartHistoryDto();
        //    newObject.Status = cartInfo.Status;
        //}
        var result = query.Select(item => new CartHistoryDto()
        {
            Status = item.Status,
            DateOrder = item.DateOrder.Value.ToString("MM/dd/yyyy HH:mm"),
            TotalPrice = item.CartDetails.Sum(s => s.Quantity * s.ProductInfo.Price),
            CartDetails = item.CartDetails.Select(cd => new CartdetailHistoryDto()
            {
               //Image = cd.ProductInfo.ImageMenu,
               ProductName = cd.ProductInfo.Name,
               Quantity = cd.Quantity,
               Price = cd.ProductInfo.Price,
            })
        });
        return await result.ToListAsync();
    }

    public async Task<CartInfo?> GetCartInfoDetail(Guid cartInfoId)
    {
        return await _productDbContext.CartInfos
           .Where(item => item.Id == cartInfoId)
           .Include(item => item.CartDetails)
           .FirstOrDefaultAsync();
    }

}