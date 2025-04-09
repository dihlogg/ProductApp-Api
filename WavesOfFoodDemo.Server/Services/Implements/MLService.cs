using AutoMapper;
using Microsoft.ML;
using WavesOfFoodDemo.Server.Dtos.Clustering;

namespace WavesOfFoodDemo.Server.Services.Implements
{
    public class MLService : IMLService
    {
        private readonly ILogger<MLService> _logger;
        private readonly IMapper _mapper;
        private readonly IProductInfoService _productInfoService;
        private readonly IRedisService _redisService;
        private readonly MLContext _mlContext;

        public MLService(
            ILogger<MLService> logger,
            IMapper mapper,
            IProductInfoService productInfoService,
            IRedisService redisService)
        {
            _logger = logger;
            _mapper = mapper;
            _productInfoService = productInfoService;
            _redisService = redisService;
            _mlContext = new MLContext();
        }

        public async Task<List<ProductFeatureDto>> GetSimilarProductsAsync(Guid productId)
        {
            var allProducts = await _productInfoService.GetProductFeaturesAsync();
            var targetProduct = allProducts.FirstOrDefault(p => p.Id == productId);

            if (targetProduct == null || targetProduct.CategoryId == null)
                return new List<ProductFeatureDto>();

            var sameCategoryProducts = allProducts
                .Where(p => p.CategoryId == targetProduct.CategoryId && p.Id != productId)
                .ToList();

            var clusterId = await _redisService.GetClusterDataAsync(targetProduct.CategoryId.Value, targetProduct.Id);

            if (clusterId == null)
                return new List<ProductFeatureDto>();

            var similarProducts = new List<ProductFeatureDto>();
            similarProducts.Add(targetProduct);
            foreach (var product in sameCategoryProducts)
            {
                var otherClusterId = await _redisService.GetClusterDataAsync(product.CategoryId.Value, product.Id);
                if (otherClusterId == clusterId)
                {
                    similarProducts.Add(product);
                }
            }
            return similarProducts.Take(4).ToList();
        }


        public async Task UpdateClustersAsync()
        {
            var allProducts = await _productInfoService.GetProductFeaturesAsync();
            var productsByCategory = allProducts
                .Where(p => p.CategoryId.HasValue)
                .GroupBy(p => p.CategoryId.Value);

            foreach (var group in productsByCategory)
            {
                var categoryId = group.Key;
                var products = group.ToList();

                var mlData = products.Select(p => new ProductFeatureMLDto
                {
                    Price = (float)p.Price * 100,
                    CpuType = p.CpuType ?? "",
                    RamType = p.RamType ?? "",
                    RomType = p.RomType ?? "",
                    ScreenSize = p.ScreenSize ?? "",
                    BateryCapacity = p.BateryCapacity ?? "",
                    DetailsType = p.DetailsType ?? "",
                    ConnectType = p.ConnectType ?? ""
                }).ToList();

                var dataView = _mlContext.Data.LoadFromEnumerable(mlData);

                var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding(new[]
                {
                new InputOutputColumnPair(nameof(ProductFeatureMLDto.CpuType)),
                new InputOutputColumnPair(nameof(ProductFeatureMLDto.RamType)),
                new InputOutputColumnPair(nameof(ProductFeatureMLDto.RomType)),
                new InputOutputColumnPair(nameof(ProductFeatureMLDto.ScreenSize)),
                new InputOutputColumnPair(nameof(ProductFeatureMLDto.BateryCapacity)),
                new InputOutputColumnPair(nameof(ProductFeatureMLDto.DetailsType)),
                new InputOutputColumnPair(nameof(ProductFeatureMLDto.ConnectType))
            })
                .Append(_mlContext.Transforms.Concatenate("Features",
                    nameof(ProductFeatureMLDto.Price),
                    nameof(ProductFeatureMLDto.CpuType),
                    nameof(ProductFeatureMLDto.RamType),
                    nameof(ProductFeatureMLDto.RomType),
                    nameof(ProductFeatureMLDto.ScreenSize),
                    nameof(ProductFeatureMLDto.BateryCapacity),
                    nameof(ProductFeatureMLDto.DetailsType),
                    nameof(ProductFeatureMLDto.ConnectType)
                ))
                .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(_mlContext.Clustering.Trainers.KMeans("Features", numberOfClusters: 3));

                var model = pipeline.Fit(dataView);
                var predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductFeatureMLDto, ProductPrediction>(model);

                for (int i = 0; i < products.Count; i++)
                {
                    var prediction = predictionEngine.Predict(mlData[i]);
                    await _redisService.SetClusterDataAsync(categoryId, products[i].Id, (int)prediction.ClusterId);
                }
            }
        }
    }

}
