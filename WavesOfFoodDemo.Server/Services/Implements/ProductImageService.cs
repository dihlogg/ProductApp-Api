using AutoMapper;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;
using WavesOfFoodDemo.Server.Infrastructures.Repositories;

namespace WavesOfFoodDemo.Server.Services.Implements
{
    public class ProductImageService : IProductImageService
    {
        private readonly ILogger<ProductImageService> _logger;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IMapper _mapper;

        public ProductImageService(IProductImageRepository productImageRepository, ILogger<ProductImageService> logger, IMapper mapper)
        {
            _productImageRepository = productImageRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<List<ProductImageDto>> GetProductImageDtosAsync()
        {
            try
            {
                var data = await _productImageRepository.GetAllAsync();
                return _mapper.Map<List<ProductImageDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<bool> AddProductImageAsync(ProductImageCreateDto productImageCreateDto)
        {
            try
            {
                var info = _mapper.Map<ProductImage>(productImageCreateDto);
                return await _productImageRepository.AddAsync(info);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<bool?> EditProductImageAsync(ProductImageDto productImageDto)
        {
            try
            {
                var productInfo = await _productImageRepository.GetByIdAsync(productImageDto.Id);
                if (productInfo == null)
                {
                    return null;
                }
                var infoUpdate = _mapper.Map<ProductImage>(productImageDto);
                var result = await _productImageRepository.UpdateAsync(infoUpdate);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<bool?> RemoveProductImageDtosAsync(Guid id)
        {
            try
            {
                return await _productImageRepository.DeleteByKey(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
