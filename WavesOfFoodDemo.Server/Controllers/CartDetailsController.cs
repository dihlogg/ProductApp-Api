using Microsoft.AspNetCore.Mvc;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Services;

namespace WavesOfFoodDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartDetailsController : ControllerBase
    {
        private readonly ILogger<CartDetailsController> _logger;
        private readonly ICartDetailsService _cartDetailsService;

        public CartDetailsController(
            ILogger<CartDetailsController> logger,
            ICartDetailsService cartDetailsService)
        {
            _logger = logger;
            _cartDetailsService = cartDetailsService;
        }

        [HttpGet("GetCartDetails")]
        public async Task<IActionResult> GetCartDetails()
        {
            try
            {
                var data = await _cartDetailsService.GetCartDetailsDtosAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("PostCartDetails")]
        public async Task<IActionResult> PostCartDetails(CartDetailsCreateDto cartDetailsCreateDto)
        {
            try
            {
                var data = await _cartDetailsService.AddCartDetailsAsync(cartDetailsCreateDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutCartDetails")]
        public async Task<IActionResult> PutCartDetails(CartDetailsDto cartDetailsDto)
        {
            try
            {
                var data = await _cartDetailsService.EditCartDetailsAsync(cartDetailsDto);
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

        [HttpDelete("DeleteCartDetails/{id}")]
        public async Task<IActionResult> DeleteCartDetails(Guid id)
        {
            try
            {
                var data = await _cartDetailsService.RemoveCartDetailsDtosAsync(id);
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