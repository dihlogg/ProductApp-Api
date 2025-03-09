using AutoMapper;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;
using WavesOfFoodDemo.Server.Infrastructures;

namespace WavesOfFoodDemo.Server.Services;

public class ProductInfoService : IProductInfoService
{
    private readonly ILogger<ProductInfoService> _logger;
    private readonly IProductInfoRepository _productInfoRepository;
    private readonly IMapper _mapper;

    public ProductInfoService(IProductInfoRepository productInfoRepository, ILogger<ProductInfoService> logger, IMapper mapper)
    {
        _productInfoRepository = productInfoRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<ProductInfoDto>> GetProductInfoDtosAsync()
    { 
        try
        {
            var data = await _productInfoRepository.GetProductAsync();
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<bool> AddProductInfoAsync(ProductInfoCreateDto productInfoCreateDto)
    {
        try
        {
            var info = _mapper.Map<ProductInfo>(productInfoCreateDto);
            return await _productInfoRepository.AddProductAsync(info);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
    //public async Task<bool?> EditProductInfoAsync(ProductInfoDto productInfoDto)
    //{
    //    try
    //    {
    //        var productInfo = await _productInfoRepository.GetByIdAsync(productInfoDto.Id);
    //        if (productInfo == null)
    //        {
    //            return null;
    //        }
    //        var infoUpdate = _mapper.Map<ProductInfo>(productInfoDto);
    //        var result = await _productInfoRepository.UpdateAsync(infoUpdate);
    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex.Message);
    //        throw;
    //    }
    //}
    public async Task<bool?> EditProductInfoAsync(ProductInfoDto productInfoDto)
    {
        try
        {
            var productInfo = await _productInfoRepository.GetByIdAsync(productInfoDto.Id);
            if (productInfo == null) return null;

            productInfo.Name = productInfoDto.Name;
            productInfo.Price = productInfoDto.Price;
            productInfo.Description = productInfoDto.Description;
            productInfo.Quantity = productInfoDto.Quantity;
            productInfo.CategoryId = productInfoDto.CategoryId;

            var imageDtos = productInfoDto.ProductImages
                .Select(pi => new ProductImageDto { ImageUrl = pi.ImageUrl })
                .ToList();
            var result = await _productInfoRepository.UpdateProductImagesAsync(productInfo, imageDtos);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<bool?> RemoveProductInfoDtosAsync(Guid id)
    {
        try
        {
            return await _productInfoRepository.DeleteByKey(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<List<ProductInfoDto>> SearchProductInfoDtosAsync(string productName)
    {
        try
        {
            var data = await _productInfoRepository.SearchProductInfoDtosAsync(productName);
            return _mapper.Map<List<ProductInfoDto>>(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task<List<ProductInfoDto>> GetPopularProducts()
    {
        try
        {
            return await _productInfoRepository.GetPopularProducts();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
    public async Task<ProductInfoDto?> GetProductDetailsById(Guid id)
    {
        try
        {
            return await _productInfoRepository.GetProductDetailsByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}