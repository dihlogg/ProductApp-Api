using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.DataContext;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Dtos.Clustering;
using WavesOfFoodDemo.Server.Entities;

namespace WavesOfFoodDemo.Server.Infrastructures;

public class ProductInfoRepository : GenericRepository<ProductInfo>, IProductInfoRepository
{
    public ProductInfoRepository(ProductDbContext productDbContext) : base(productDbContext)
    {
    }

    public async Task<List<ProductInfoDto>> SearchProductInfoDtosAsync(string productName)
    {
        return await _productDbContext.ProductInfos
            .Where(p => p.Name.ToLower().Contains(productName.ToLower()))
            .Select(p => new ProductInfoDto
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
                ProductImages = p.ProductImages
                    .OrderBy(img => img.DisplayOrder)
                    .Select(img => new ProductImageCreateDto
                    {
                        ImageUrl = img.ImageUrl
                    }).ToList()
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<ProductInfo>> GetPopularProducts()
    {
        var query = _productDbContext.CartDetails
            .Include(cd => cd.ProductInfo.ProductImages)
            .AsQueryable();
        var queryGroup = query.GroupBy(s => s.ProductId);
        var newQuery = queryGroup
            .Select(s => new
            {
                ProductId = s.Key,
                ProductInfoAfterGroup = s.First().ProductInfo,
                SumQuantity = s.Sum(t => t.Quantity),
            })
            .OrderByDescending(s => s.SumQuantity)
            .Take(5);
        var dataGroup = await newQuery.ToListAsync();
        return dataGroup.Select(s => s.ProductInfoAfterGroup).ToList();
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
    public async Task<List<ProductInfoDto>> GetProductAsync()
    {
        return await _productDbContext.ProductInfos
            .Select(p => new ProductInfoDto
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
                }).ToList()
            })
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
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ProductInfoDto>> GetProductsByCategoryIdAsync(Guid categoryId)
    {
        return await _productDbContext.ProductInfos
            .Where(p => p.CategoryId == categoryId)
            .Select(p => new ProductInfoDto
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
                }).ToList()
            })
            .ToListAsync(); // trả về list sp
    }
    public async Task UpdateProductAsync(ProductInfo productInfo)
    {
        _productDbContext.ProductInfos.Update(productInfo);
        await _productDbContext.SaveChangesAsync();
    }
    public async Task<List<ProductFeatureDto>> GetProductFeaturesAsync()
    {
        var productList = await _productDbContext.ProductInfos
        .Select(p => new ProductFeatureDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            Quantity = p.Quantity,
            CategoryId = p.CategoryId,
            ProductImages = p.ProductImages.OrderBy(s => s.DisplayOrder).Select(img => new ProductImageCreateDto
            {
                ImageUrl = img.ImageUrl
            }).ToList(),

            // recommend criteria
            CpuType = p.CpuType,
            RamType = p.RamType,
            RomType = p.RomType,
            ScreenSize = p.ScreenSize,
            BateryCapacity = p.BateryCapacity,
            DetailsType = p.DetailsType,
            ConnectType = p.ConnectType,
        })
        .ToListAsync();

        return productList;
    }
}