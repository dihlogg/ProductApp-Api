using AutoMapper;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;
using WavesOfFoodDemo.Server.Infrastructures.Repositories;

namespace WavesOfFoodDemo.Server.Services.Implements
{
    public class ProductInfoHistoryService : IProductInfoHistoryService
    {
        private readonly ILogger<ProductInfoHistoryService> _logger;
        private readonly IProductInfoHistoryRepository _productInfoHistoryRepository;
        private readonly IMapper _mapper;

        public ProductInfoHistoryService(IProductInfoHistoryRepository productInfoHistoryRepository, ILogger<ProductInfoHistoryService> logger, IMapper mapper)
        {
            _productInfoHistoryRepository = productInfoHistoryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task GenerateDailyTopSellingProducts()
        {
            await _productInfoHistoryRepository.DeleteAllProductInfoHistory();

            var topSales = await _productInfoHistoryRepository.GetTopSellingProductsYesterday();
            var products = await _productInfoHistoryRepository.GetProductInfosByIds(topSales.Select(x => x.ProductId));

            var historyRecords = products.Select(p =>
            {
                var newHistoryId = Guid.NewGuid();

                var history = new ProductInfoHistory
                {
                    Id = newHistoryId,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Quantity = p.Quantity,
                    BateryCapacity = p.BateryCapacity,
                    ConnectType = p.ConnectType,
                    CpuType = p.CpuType,
                    DetailsType = p.DetailsType,
                    RamType = p.RamType,
                    RomType = p.RomType,
                    ScreenSize = p.ScreenSize,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateBy = Guid.Empty,
                    UpdateBy = Guid.Empty,
                    ProductImages = p.ProductImages
                    .Select((img, index) => new ProductImage
                    {
                        ImageUrl = img.ImageUrl
                    })
                    .ToList()
                };

                return history;
            }).ToList();

            await _productInfoHistoryRepository.SaveProductInfoHistory(historyRecords);
        }
        public async Task<List<ProductInfoHistoryDto>> GetRandomTop10Products()
        {
            var randomProducts = await _productInfoHistoryRepository.GetProductInfoHistories();
            if (randomProducts.Count < 10)
            {
                return randomProducts;
            }
            return randomProducts.OrderBy(_ => Guid.NewGuid()).Take(10).ToList();
        }
    }
}
