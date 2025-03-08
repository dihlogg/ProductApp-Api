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
    public async Task<bool> AddProductAsync(ProductInfo productInfo)
    {
        _productDbContext.ProductInfos.Add(productInfo);
        if (productInfo.ProductImages != null && productInfo.ProductImages.Any())
        {
            _productDbContext.ProductImages.AddRange(productInfo.ProductImages);
        }

        await _productDbContext.SaveChangesAsync();
        return true;
    }
    public async Task<List<ProductInfo>> GetProductAsync()
    {
        return await _productDbContext.ProductInfos
            .Include(p => p.ProductImages)
            .ToListAsync();
    }
    public async Task<bool> UpdateProductImagesAsync(ProductInfo productInfo, List<ProductImageDto> newImages)
    {
        var existingImages = _productDbContext.ProductImages.Where(img => img.ProductInfoId == productInfo.Id).ToList();

        var newImageUrls = newImages.Select(img => img.ImageUrl).ToList();
        var imagesToRemove = existingImages.Where(img => !newImageUrls.Contains(img.ImageUrl)).ToList();

        if (imagesToRemove.Any())
        {
            _productDbContext.ProductImages.RemoveRange(imagesToRemove);
        }

        // Thêm ảnh mới nếu chưa có trong DB
        foreach (var newImage in newImages)
        {
            if (!existingImages.Any(img => img.ImageUrl == newImage.ImageUrl))
            {
                _productDbContext.ProductImages.Add(new ProductImage
                {
                    ProductInfoId = productInfo.Id,
                    ImageUrl = newImage.ImageUrl
                });
            }
        }

        await _productDbContext.SaveChangesAsync();
        return true;
    }
    public async Task<ProductInfoDto?> GetProductDetailsByIdAsync(Guid id)
    {
        return await _productDbContext.ProductInfos
            .Where(p => p.Id == id)
            .Select(p => new ProductInfoDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Quantity = p.Quantity,
                CategoryId = p.CategoryId,
                ProductImages = p.ProductImages.Select(img => new ProductImageCreateDto
                {
                    ImageUrl = img.ImageUrl
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }
}