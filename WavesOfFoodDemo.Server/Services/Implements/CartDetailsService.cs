using AutoMapper;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;
using WavesOfFoodDemo.Server.Infrastructures;

namespace WavesOfFoodDemo.Server.Services
{
    public class CartDetailsService : ICartDetailsService
    {
        private readonly ILogger<CartDetailsService> _logger;
        private readonly ICartDetailsRepository _cartDetailsRepository;
        private readonly IMapper _mapper;

        public CartDetailsService(
            ICartDetailsRepository cartDetailsRepository, 
            ILogger<CartDetailsService> logger, 
            IMapper mapper)
        {
            _cartDetailsRepository = cartDetailsRepository;
            _logger = logger;
            _mapper = mapper;
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
    }
}
