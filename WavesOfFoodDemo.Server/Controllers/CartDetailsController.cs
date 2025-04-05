using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Hubs;
using WavesOfFoodDemo.Server.Services;

namespace WavesOfFoodDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartDetailsController : ControllerBase
    {
        private readonly ILogger<CartDetailsController> _logger;
        private readonly ICartDetailsService _cartDetailsService;
        private readonly IProductInfoService _productInfoService;
        private readonly IRedisService _redisService;
        private const string CART_KEY_PREFIX = "CartRedis:";
        private readonly IHubContext<CartHub> _hubContext;

        public CartDetailsController(
            ILogger<CartDetailsController> logger,
            ICartDetailsService cartDetailsService,
            IRedisService redisService,
            IHubContext<CartHub> hubContext,
            IProductInfoService productInfoService)
        {
            _logger = logger;
            _cartDetailsService = cartDetailsService;
            _redisService = redisService;
            _hubContext = hubContext;
            _productInfoService = productInfoService;
        }
        [HttpPost("Redis/PostCartRedis")]
        public async Task<IActionResult> PostCartRedis([FromBody] CartDetailsCreateDto cartItem)
        {
            try
            {
                var key = $"CartRedis:{cartItem.UserId}:{cartItem.ProductId}";

                // create object
                var cartValue = new
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity
                };

                // serialize object to json
                var serializedValue = JsonSerializer.Serialize(cartValue);

                // lưu redis & set token chết
                await _redisService.SetValueAsync(key, serializedValue, TimeSpan.FromDays(30));

                // Gửi event tới tất cả tab của user đó
                await _hubContext.Clients.Group(cartItem.UserId.ToString())
                    .SendAsync("ReceiveCartUpdate", "add", cartValue);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }

        [HttpGet("Redis/GetCartItems/{userId}")]
        public async Task<IActionResult> GetCartItems(Guid userId)
        {
            try
            {
                var pattern = $"CartRedis:{userId}:*";
                var keys = await _redisService.GetAllKeysAsync(pattern);

                var cartItems = new List<object>();
                foreach (var key in keys)
                {
                    var value = await _redisService.GetValueAsync(key);
                    if (!string.IsNullOrEmpty(value))
                    {
                        cartItems.Add(JsonSerializer.Deserialize<object>(value));
                    }
                }

                return Ok(cartItems);
            }
            catch
            {
                return BadRequest(false);
            }
        }
        [HttpDelete("Redis/DeleteProductFromCartRedis/user/{userId}/product/{productId}")]
        public async Task<IActionResult> DeleteProductFromCartRedis(Guid userId, Guid productId)
        {
            try
            {
                var key = $"CartRedis:{userId}:{productId}";
                var result = await _redisService.DeleteKeyAsync(key);
                if (result)
                {
                    // Gửi thông báo xóa qua SignalR
                    await _hubContext.Clients.Group(userId.ToString())
                        .SendAsync("ReceiveCartUpdate", "delete", new { ProductId = productId });
                }
                return Ok(result);
            }
            catch
            {
                return Ok(false);
            }
        }

        [HttpPost("Redis/PostCartDetailsRedis")]
        public async Task<IActionResult> PostCartDetailsRedis([FromBody] CartDetailsRequestDto request)
        {
            try
            {
                if (request.UserId == null || request.Products == null || !request.Products.Any())
                {
                    return BadRequest(false);
                }

                bool result = await _cartDetailsService.ProcessOrderAsync(request.UserId.Value, request.Products);
                return result ? Ok(true) : BadRequest(new { Success = false, Message = "Order processing failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [HttpPost("PostCartDetails/{cartId}")]
        public async Task<IActionResult> PostCartDetails(Guid cartId, CartDetailsCreateDto cartDetailsCreateDto)
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