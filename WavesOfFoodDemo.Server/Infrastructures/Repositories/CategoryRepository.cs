using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;
using WavesOfFoodDemo.Server.Infrastructures;

namespace WavesOfFoodDemo.Server.Infrastructures.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ProductDbContext productDbContext) : base(productDbContext) { }
    public async Task<List<Category>> SearchCategoryDtosAsync(string categoryName)
        {
            var query = _productDbContext.Categories.AsQueryable();
            query = query.Where(s => s.CategoryName.Contains(categoryName));
            return await query.AsNoTracking().ToListAsync();
        }
    }
}

