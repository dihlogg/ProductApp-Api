using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using WavesOfFoodDemo.Server.AppSettings;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Dtos.CartDetails;
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

        public CartDetailsController(
            ILogger<CartDetailsController> logger,
            ICartDetailsService cartDetailsService,
            IRedisService redisService,
            IProductInfoService productInfoService)
        {
            _logger = logger;
            _cartDetailsService = cartDetailsService;
            _redisService = redisService;
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
                await _redisService.SetValueAsync(key, serializedValue, TimeSpan.FromDays(7));

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
        [HttpDelete("Redis/DeleteCartRedis/user/{userId}/product/{productId}")]
        public async Task<IActionResult> DeleteCartRedis(Guid userId, Guid productId)
        {
            try
            {
                var key = $"CartRedis:{userId}:{productId}";
                var result = await _redisService.DeleteKeyAsync(key);
                return Ok(result);
            }
            catch
            {
                return Ok(false);
            }
        }

        [HttpGet("GetCartDetails")]
        public async Task<IActionResult> GetCartDetails()
        {
            try
            {
                List<CartDetailsDto> cartDetails;
                var key = $"{nameof(CartDetailsController)}:demo";
                var data = await _redisService.GetValueAsync(key);
                if (!string.IsNullOrEmpty(data))
                {
                    cartDetails = JsonSerializer.Deserialize<List<CartDetailsDto>>(data);
                    return Ok(cartDetails);
                }
                cartDetails = await _cartDetailsService.GetCartDetailsDtosAsync();
                data = JsonSerializer.Serialize(cartDetails);
                await _redisService.SetValueAsync(key, data, TimeSpan.FromSeconds(10));
                return Ok(cartDetails);
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