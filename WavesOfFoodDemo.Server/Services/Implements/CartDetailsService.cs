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

        public CartDetailsService(
            ICartDetailsRepository cartDetailsRepository,
            ILogger<CartDetailsService> logger,
            IMapper mapper,
            IRedisService redisService,
            IHubContext<CartHub> hubContext,
            ICartInfoRepository cartInfoRepository,
            IProductInfoRepository productInfoRepository)
        {
            _cartDetailsRepository = cartDetailsRepository;
            _logger = logger;
            _mapper = mapper;
            _redisService = redisService;
            _hubContext = hubContext;
            _cartInfoRepository = cartInfoRepository;
            _productInfoRepository = productInfoRepository;
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
            {
                try
                {
                    var cartItems = new Dictionary<Guid, int>();

                    // get cart details fr redis
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

                    // add product to cartinfo
                    CartInfo cartInfo = new CartInfo();
                    cartInfo.Id = Guid.NewGuid();
                    cartInfo.Status = StatusOrderConst.NewConst;
                    cartInfo.UserId = userId;
                    cartInfo.DateOrder = DateTime.UtcNow;
                    _cartInfoRepository.Add(cartInfo);

                    CartDetails cartDetails = new CartDetails();
                    cartDetails.Id = Guid.NewGuid();


                    // valid product in db
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
                    }

                    foreach (var item in products)
                    {
                        var key = $"CartRedis:{userId}:{item.ProductId}";
                        await _redisService.DeleteKeyAsync(key);
                    }

                    // gởi thông báo signalR
                    await _hubContext.Clients.Group(userId.ToString()).SendAsync("ReceiveCartUpdate", "delete");

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw;
                }
            }

        }
    }
}
