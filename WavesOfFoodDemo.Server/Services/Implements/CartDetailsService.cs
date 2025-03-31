using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WavesOfFoodDemo.Server.AppSettings;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;
using WavesOfFoodDemo.Server.Hubs;
using WavesOfFoodDemo.Server.Infrastructures;
using WavesOfFoodDemo.Server.Services.Implements;

namespace WavesOfFoodDemo.Server.Services
{
    public class CartDetailsService : ICartDetailsService
    {
        private readonly ILogger<CartDetailsService> _logger;
        private readonly ICartDetailsRepository _cartDetailsRepository;
        private readonly ICartInfoRepository _cartInfoRepository;
        private readonly IProductInfoRepository _productInfoRepository;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IHubContext<CartHub> _hubContext;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public CartDetailsService(
            ICartDetailsRepository cartDetailsRepository,
            ILogger<CartDetailsService> logger,
            IMapper mapper,
            IRedisService redisService,
            IHubContext<CartHub> hubContext,
            ICartInfoRepository cartInfoRepository,
            IProductInfoRepository productInfoRepository,
            IUserInfoRepository userInfoRepository,
            IHttpClientFactory httpClientFactory)
        {
            _cartDetailsRepository = cartDetailsRepository;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _hubContext = hubContext;
            _cartInfoRepository = cartInfoRepository;
            _productInfoRepository = productInfoRepository;
            _userInfoRepository = userInfoRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> AddCartDetailsAsync(CartDetailsCreateDto cartDetailsCreateDto)
        {
            try
            {
                var info = _mapper.Map<CartDetails>(cartDetailsCreateDto);
                return await _cartDetailsRepository.AddAsync(info);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<bool?> EditCartDetailsAsync(CartDetailsDto cartDetailsDto)
        {
            try
            {
                var productInfo = await _cartDetailsRepository.GetByIdAsync(cartDetailsDto.Id);
                if (productInfo == null)
                {
                    return null;
                }
                var infoUpdate = _mapper.Map<CartDetails>(cartDetailsDto);
                var result = await _cartDetailsRepository.UpdateAsync(infoUpdate);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<List<CartDetailsDto>> GetCartDetailsDtosAsync()
        {
            try
            {
                var data = await _cartDetailsRepository.GetAllAsync();
                return _mapper.Map<List<CartDetailsDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<bool?> RemoveCartDetailsDtosAsync(Guid id)
        {
            {
                try
                {
                    return await _cartDetailsRepository.DeleteByKey(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }
        }

        public async Task<bool> ProcessOrderAsync(Guid userId, List<CartDetailsRequestCreateDto> products)
        {
            try
            {
                var cartItems = new Dictionary<Guid, int>();

                // get product fr redis
                foreach (var product in products)
                {
                    var key = $"CartRedis:{userId}:{product.ProductId}";
                    var cartData = await _redisService.GetValueAsync(key);
                    if (!string.IsNullOrEmpty(cartData))
                    {
                        var cartItem = JsonSerializer.Deserialize<CartDetailsRequestCreateDto>(cartData);
                        cartItems[product.ProductId] = cartItem.Quantity;
                    }
                }

                // add new product to cart info
                CartInfo cartInfo = new CartInfo
                {
                    Id = Guid.NewGuid(),
                    Status = StatusOrderConst.NewConst,
                    UserId = userId,
                    DateOrder = DateTime.UtcNow
                };
                _cartInfoRepository.Add(cartInfo);

                CartDetails cartDetails = new CartDetails { Id = Guid.NewGuid() };
                var productList = new List<object>();
                decimal totalPrice = 0;
                int totalQuantity = 0;

                // validate and process products in DB
                foreach (var item in products)
                {
                    var productInDb = await _productInfoRepository.GetByIdAsync(item.ProductId);
                    if (productInDb == null)
                    {
                        throw new Exception($"Product {item.ProductId} not found");
                    }

                    int cartQuantity = cartItems.GetValueOrDefault(item.ProductId, 0);
                    int newQuantity = (productInDb.Quantity ?? 0) - cartQuantity;

                    if (newQuantity < 0)
                    {
                        throw new Exception($"Order quantity for product {item.ProductId} exceeds stock. Available: {productInDb.Quantity ?? 0}");
                    }

                    // update product quantity
                    productInDb.Quantity = newQuantity;
                    cartDetails.CartId = cartInfo.Id;
                    cartDetails.ProductId = item.ProductId;
                    cartDetails.Quantity = item.Quantity;
                    _cartDetailsRepository.Add(cartDetails);

                    await _productInfoRepository.UpdateProductAsync(productInDb);

                    // add product details for payload to n8n
                    productList.Add(new
                    {
                        name = productInDb.Name,
                        quantity = item.Quantity,
                        price = productInDb.Price
                    });

                    totalPrice += productInDb.Price * item.Quantity;
                    totalQuantity += item.Quantity;
                }

                // clear cart fr redis
                foreach (var item in products)
                {
                    var key = $"CartRedis:{userId}:{item.ProductId}";
                    await _redisService.DeleteKeyAsync(key);
                }

                // send notify group signalR
                await _hubContext.Clients.Group(userId.ToString()).SendAsync("ReceiveCartUpdate", "delete");

                // call func send mail order n8n webhook
                await SendOrderToWebhookAsync(userId, cartInfo, productList, totalQuantity, totalPrice);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        private async Task SendOrderToWebhookAsync(Guid userId, CartInfo cartInfo, List<object> productList, int totalQuantity, decimal totalPrice)
        {
            using (var httpClient = new HttpClient())
            {
                var user = await _userInfoRepository.GetByIdAsync(userId);
                var webhookUrl = "http://localhost:5678/webhook/order";
                // payload for n8n
                var orderData = new
                {
                    body = new
                    {
                        delivery = new
                        {
                            order_id = cartInfo.Id.ToString(),
                            order_date = cartInfo.DateOrder?.ToString("yyyy-MM-dd"),
                            recipient = new
                            {
                                name = user?.UserFullName,
                                phone = user?.UserPhone,
                                address = user?.UserAddress,
                                email = user?.UserName,
                                country = "Việt Nam"
                            },
                            shipping = new
                            {
                                provider = "Giao hàng tiết kiệm",
                            }
                        },
                        products = productList,
                        totalQuantity = totalQuantity,
                        totalPrice = totalPrice
                    }
                };

                var jsonData = JsonSerializer.Serialize(orderData);
                var payload = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                // send post request tới n8n
                var response = await httpClient.PostAsync(webhookUrl, payload);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to send order notification to webhook: {response.ReasonPhrase}");
                }
            }
        }
    }
}
