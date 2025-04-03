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

        public MLService(ILogger<MLService> logger, IMapper mapper, IProductInfoService productInfoService, IRedisService redisService)
        {
            _logger = logger;
            _mapper = mapper;
            _productInfoService = productInfoService;
            _redisService = redisService;
        }
        public async Task<List<ProductFeatureDto>> GetFeaturedProductsAsync()
        {
            var productList = await _productInfoService.GetProductFeaturesAsync();

            // get clusterId fr redis 6380
            var productClusters = new List<(ProductFeatureDto Product, int ClusterId)>();

            foreach (var product in productList)
            {
                var clusterId = await _redisService.GetClusterDataAsync(product.Id);
                if (clusterId.HasValue)
                {
                    productClusters.Add((product, clusterId.Value));
                }
            }

            // group product
            var clusterStats = productClusters
                .GroupBy(p => p.ClusterId)
                .Select(g => new
                {
                    ClusterId = g.Key,
                    AvgSoldQuantity = g.Average(p => p.Product.SoldQuantity),
                    Products = g.Select(p => p.Product).ToList()
                });

            var featuredProducts = clusterStats
                .OrderByDescending(c => c.AvgSoldQuantity)
                .FirstOrDefault()?.Products
                .Take(6)
                .ToList() ?? new List<ProductFeatureDto>();

            return featuredProducts;
        }

        // train K-means
        public async Task UpdateClustersAsync()
        {
            var productList = await _productInfoService.GetProductFeaturesAsync();

            var mlData = productList.Select(p => new ProductFeatureMLDto
            {
                Price = (float)p.Price,
                OrderCount = p.OrderCount,
                SoldQuantity = p.SoldQuantity
            }).ToList();

            var mlContext = new MLContext();
            var dataView = mlContext.Data.LoadFromEnumerable(mlData);

            var pipeline = mlContext.Transforms
                .Concatenate("Features", nameof(ProductFeatureMLDto.Price), nameof(ProductFeatureMLDto.OrderCount), nameof(ProductFeatureMLDto.SoldQuantity))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Clustering.Trainers.KMeans("Features", numberOfClusters: 3));

            var model = pipeline.Fit(dataView);
            var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductFeatureMLDto, ProductPrediction>(model);

            foreach (var (data, index) in mlData.Select((data, index) => (data, index)))
            {
                var prediction = predictionEngine.Predict(data);
                await _redisService.SetClusterDataAsync(productList[index].Id, (int)prediction.ClusterId);
            }
        }

    }
}
