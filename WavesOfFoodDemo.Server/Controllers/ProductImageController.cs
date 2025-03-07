using Microsoft.AspNetCore.Mvc;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Services;

namespace WavesOfFoodDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductImageController : ControllerBase
    {
        private readonly ILogger<ProductImageController> _logger;
        private readonly IProductImageService _productImageService;

        public ProductImageController(
            ILogger<ProductImageController> logger,
            IProductImageService productImageService)
        {
            _logger = logger;
            _productImageService = productImageService;
        }
        [HttpGet("GetProductImages")]
        public async Task<IActionResult> GetProductImages()
        {
            try
            {
                var data = await _productImageService.GetProductImageDtosAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostProductImage")]
        public async Task<IActionResult> PostProductImage(ProductImageCreateDto productImageCreateDto)
        {
            try
            {
                var data = await _productImageService.AddProductImageAsync(productImageCreateDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutProductImage")]
        public async Task<IActionResult> PutProductImage(ProductImageDto productImageDto)
        {
            try
            {
                var data = await _productImageService.EditProductImageAsync(productImageDto);
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

        [HttpDelete("DeleteProductImage/{id}")]
        public async Task<IActionResult> DeleteProductImage(Guid id)
        {
            try
            {
                var data = await _productImageService.RemoveProductImageDtosAsync(id);
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
    }
}
