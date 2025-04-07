using Microsoft.AspNetCore.Mvc;
using WavesOfFoodDemo.Server.Services;

namespace WavesOfFoodDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductInfoHistoryController : ControllerBase
    {
        private readonly ILogger<ProductInfoHistoryController> _logger;
        private readonly IProductInfoHistoryService _productInfoHistoryService;

        public ProductInfoHistoryController(
            ILogger<ProductInfoHistoryController> logger,
            IProductInfoHistoryService productInfoHistoryService)
        {
            _logger = logger;
            _productInfoHistoryService = productInfoHistoryService;
        }
        [HttpGet("GetFeaturedProductHistory")]
        public async Task<IActionResult> GetFeaturedProducts()
        {
            try
            {
                var data = await _productInfoHistoryService.GetRandomTop10Products();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GenerateProductHistory")]
        public async Task<IActionResult> GenerateProductHistory()
        {
            await _productInfoHistoryService.GenerateDailyTopSellingProducts();
            return Ok(new { Message = "Top 30 products history generated successfully." });
        }
    }
}
