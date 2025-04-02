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

        public MLService(ILogger<MLService> logger, IMapper mapper, IProductInfoService productInfoService)
        {
            _logger = logger;
            _mapper = mapper;
            _productInfoService = productInfoService;
        }
        public async Task<List<ProductFeatureDto>> GetFeaturedProductsAsync()
        {
            var productList = await _productInfoService.GetProductFeaturesAsync();

            // tranfer dta to features ML .Net
            var mlData = productList.Select(p => new ProductFeatureMLDto
            {
                Price = (float)p.Price,
                OrderCount = p.OrderCount,
                SoldQuantity = p.SoldQuantity
            }).ToList();

            if (mlData.Count < 3) // product có cart sts == completed < 3 => trả về list product hiện tại k cần cluster
            {
                return productList.Take(6).ToList();
            }

            var mlContext = new MLContext();
            // get data fr mlData
            var dataView = mlContext.Data.LoadFromEnumerable(mlData);

            var pipeline = mlContext.Transforms
                .Concatenate("Features", nameof(ProductFeatureMLDto.Price), nameof(ProductFeatureMLDto.OrderCount), nameof(ProductFeatureMLDto.SoldQuantity))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Clustering.Trainers.KMeans("Features", numberOfClusters: 3));

            // train k-mean model with 3 cluster
            var model = pipeline.Fit(dataView);
            // prediction cluster 
            var predictionEngine = mlContext.Model.CreatePredictionEngine<ProductFeatureMLDto, ProductPrediction>(model);

            // cluster product
            var predictions = mlData.Select((data, index) => new
            {
                Product = productList[index],
                Prediction = predictionEngine.Predict(data)
            }).ToList();

            // group product based cluster
            var clusterStats = predictions
                .GroupBy(p => p.Prediction.ClusterId)
                .Select(g => new
                {
                    ClusterId = g.Key,
                    AvgSoldQuantity = g.Average(p => p.Product.SoldQuantity),
                    Products = g.Select(p => p.Product).ToList()
                });

            var featuredProducts = clusterStats.OrderByDescending(c => c.AvgSoldQuantity)
                                               .First().Products
                                               .Take(6)
                                               .ToList();

            return featuredProducts;
        }
    }
}
