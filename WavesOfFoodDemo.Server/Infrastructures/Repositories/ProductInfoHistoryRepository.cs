using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures.Repositories;

public class ProductInfoHistoryRepository : GenericRepository<ProductInfoHistory>, IProductInfoHistoryRepository
{
    public ProductInfoHistoryRepository(ProductDbContext productDbContext) : base(productDbContext)
    {
    }
    public async Task<List<ProductInfo>> GetProductInfosByIds(IEnumerable<Guid> productIds)
    {
        return await _productDbContext.ProductInfos
       .Include(p => p.ProductImages)
       .Where(p => productIds.Contains(p.Id))
       .ToListAsync();
    }

    public async Task<List<(Guid ProductId, int QuantitySold)>> GetTopSellingProductsYesterday()
    {
        var nowVn = DateTime.UtcNow.AddHours(7);             // giờ hiện tại theo giờ Việt Nam
        var todayVn = nowVn.Date;                             // 2025-04-07 00:00:00 VN
        var yesterdayVn = todayVn.AddDays(-1);                // 2025-04-06 00:00:00 VN

        var startUtc = yesterdayVn.AddHours(-7);              // 2025-04-05 17:00:00 UTC
        var endUtc = todayVn.AddHours(-7);                    // 2025-04-06 17:00:00 UTC

        var result = await _productDbContext.CartDetails
            .Where(cd => cd.CartInfo.CreateDate >= startUtc && cd.CartInfo.CreateDate < endUtc)
            .GroupBy(cd => cd.ProductId)
            .Select(g => new { ProductId = g.Key, QuantitySold = g.Sum(x => x.Quantity) })
            .OrderByDescending(x => x.QuantitySold)
            .Take(30)
            .ToListAsync();

        return result.Select(r => (r.ProductId, r.QuantitySold)).ToList();
    }

    public async Task SaveProductInfoHistory(List<ProductInfoHistory> histories)
    {
        await _productDbContext.ProductInfoHistorys.AddRangeAsync(histories);
        await _productDbContext.SaveChangesAsync();
    }

    public async Task DeleteAllProductInfoHistory()
    {
        await _productDbContext.ProductInfoHistorys
            .ExecuteDeleteAsync();
    }
    public async Task<List<ProductInfoHistoryDto>> GetProductInfoHistories()
    {
        return await _productDbContext.ProductInfoHistorys
            .Include(p => p.ProductImages)
            .Select(p => new ProductInfoHistoryDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Quantity = p.Quantity,
                CategoryId = p.CategoryId,
                CpuType = p.CpuType,
                RamType = p.RamType,
                RomType = p.RomType,
                ScreenSize = p.ScreenSize,
                BateryCapacity = p.BateryCapacity,
                DetailsType = p.DetailsType,
                ConnectType = p.ConnectType,
                ProductImages = p.ProductImages.OrderBy(s => s.DisplayOrder).Select(img => new ProductImageCreateDto
                {
                    ImageUrl = img.ImageUrl
                }).ToList(),
            })
            .ToListAsync();
    }
}
