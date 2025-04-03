using Microsoft.AspNetCore.Mvc;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Services;

namespace WavesOfFoodDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductInfoController : ControllerBase
    {
        private readonly ILogger<ProductInfoController> _logger;
        private readonly IProductInfoService _productInfoService;
        private readonly IMLService _mlService;
        private readonly IRedisService _redisService;

        public ProductInfoController(
            ILogger<ProductInfoController> logger,
            IProductInfoService productInfoService,
            IMLService mlService,
            IRedisService redisService)
        {
            _logger = logger;
            _productInfoService = productInfoService;
            _mlService = mlService;
            _redisService = redisService;
        }

        [HttpGet("GetProductInfos")]
        public async Task<IActionResult> GetProductInfos()
        {
            try
            {
                var data = await _productInfoService.GetProductInfoDtosAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SearchProductInfos")]
        public async Task<IActionResult> SearchProductInfos(string productName)
        {
            try
            {
                var data = await _productInfoService.SearchProductInfoDtosAsync(productName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostProductInfo")]
        public async Task<IActionResult> PostProductInfo(ProductInfoCreateDto productInfoCreateDto)
        {
            try
            {
                var data = await _productInfoService.AddProductInfoAsync(productInfoCreateDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutProductInfo")]
        public async Task<IActionResult> PutProductInfo(ProductInfoDto productInfoDto)
        {
            try
            {
                var data = await _productInfoService.EditProductInfoAsync(productInfoDto);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteProductInfo/{id}")]
        public async Task<IActionResult> DeleteProductInfo(Guid id)
        {
            try
            {
                var data = await _productInfoService.RemoveProductInfoDtosAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetPopularProducts")]
        public async Task<IActionResult> GetPopularProducts()
        {
            try
            {
                var data = await _productInfoService.GetPopularProducts();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductDetailsById/{id}")]
        public async Task<IActionResult> GetProductDetailsById(Guid id)
        {
            try
            {
                var data = await _productInfoService.GetProductDetailsById(id);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductsByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(Guid categoryId)
        {
            try
            {
                var data = await _productInfoService.GetProductsByCategoryIdAsync(categoryId);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UpdateClusters")]
        public async Task<IActionResult> UpdateClusters()
        {
            await _mlService.UpdateClustersAsync();
            return Ok(new { message = "Clusters updated successfully" });
        }
        [HttpGet("GetCluster/{productId}")]
        public async Task<IActionResult> GetCluster(Guid productId)
        {
            try
            {
                var data = await _redisService.GetClusterDataAsync(productId);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(new { productId, data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductsByCluster/{clusterId}")]
        public async Task<IActionResult> GetProductsByCluster(int clusterId)
        {
            var allProductKeys = await _redisService.GetAllKeysAsync("cluster:*");

            var matchingProducts = new List<Guid>();

            foreach (var key in allProductKeys)
            {
                string productIdString = key.Replace("cluster:", "");

                if (Guid.TryParse(productIdString, out Guid productId))
                {
                    int? storedClusterId = await _redisService.GetClusterDataAsync(productId);

                    if (storedClusterId.HasValue && storedClusterId.Value == clusterId)
                    {
                        matchingProducts.Add(productId);
                    }
                }
            }

            return Ok(new { clusterId, products = matchingProducts });
        }
        [HttpGet("GetFeaturedProducts")]
        public async Task<IActionResult> GetFeaturedProducts()
        {
            try
            {
                var data = await _mlService.GetFeaturedProductsAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}