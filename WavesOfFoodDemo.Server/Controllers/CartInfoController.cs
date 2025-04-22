using Microsoft.AspNetCore.Mvc;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Dtos.CartDetails;
using WavesOfFoodDemo.Server.Services;

namespace WavesOfFoodDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartInfoController : ControllerBase
    {
        private readonly ILogger<CartInfoController> _logger;
        private readonly ICartInfoService _cartInfoService;

        public CartInfoController(
            ILogger<CartInfoController> logger,
            ICartInfoService cartInfoService)
        {
            _logger = logger;
            _cartInfoService = cartInfoService;
        }

        [HttpGet("GetCartInfos")]
        public async Task<IActionResult> GetCartInfos()
        {
            try
            {
                var data = await _cartInfoService.GetCartInfoDtosAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTransactionsByUserId")]
        public async Task<IActionResult> GetTransactions([FromQuery] Guid userId, [FromQuery] string status)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return NotFound("User not found");
                }
                var data = await _cartInfoService.GetTransactions(userId, status);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllTransactions")]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var data = await _cartInfoService.GetTransactions();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("PostCartInfo")]
        public async Task<IActionResult> PostCartInfoInfo(CartInfoCreateDto cartInfoCreateDto)
        {
            try
            {
                var data = await _cartInfoService.AddCartInfoAsync(cartInfoCreateDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutCartInfo")]
        public async Task<IActionResult> PutCartInfo(CartInfoDto cartInfoDto)
        {
            try
            {
                var data = await _cartInfoService.EditCartInfoAsync(cartInfoDto);
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

        [HttpDelete("DeleteCartInfo/{id}")]
        public async Task<IActionResult> DeleteCartInfo(Guid id)
        {
            try
            {
                var data = await _cartInfoService.RemoveCartInfoDtosAsync(id);
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

        [HttpPost("PostCartDetailInfo")]
        public async Task<IActionResult> PostCartDetailInfo(CartDetailInfoDto cartInfoCreateDto)
        {
            try
            {
                var data = await _cartInfoService.PostCartDetailInfo(cartInfoCreateDto);
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
        [HttpPut("UpdateStatusCartInfo")]
        public async Task<IActionResult> UpdateStatusCartInfo(UpdateStatusCartDetailDto updateStatusCart)
        {
            try
            {
                var data = await _cartInfoService.UpdateStatusCartInfo(updateStatusCart);
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
        [HttpGet("GetTransactionById/{id}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            try
            {
                var data = await _cartInfoService.GetTransactionByIdAsync(id);
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