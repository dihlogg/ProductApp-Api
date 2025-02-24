using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public class CartDetailsRepository : GenericRepository<CartDetails>, ICartDetailsRepository
{
    public CartDetailsRepository(ProductDbContext productDbContext) : base(productDbContext)
    {
    }

}