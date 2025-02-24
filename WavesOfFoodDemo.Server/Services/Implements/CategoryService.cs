using AutoMapper;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Entities;
using WavesOfFoodDemo.Server.Infrastructures;
using WavesOfFoodDemo.Server.Infrastructures.Repositories;

namespace WavesOfFoodDemo.Server.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<List<CategoryDto>> GetCategoryDtosAsync()
        {
            try
            {
                var data = await _categoryRepository.GetAllAsync();
                return _mapper.Map<List<CategoryDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<bool> AddCategoryAsync(CategoryCreateDto categoryCreateDto)
        {
            try
            {
                var info = _mapper.Map<Category>(categoryCreateDto);
                return await _categoryRepository.AddAsync(info);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<bool?> EditCategoryAsync(CategoryDto categoryDto)
        {
            try
            {
                var categoryInfo = await _categoryRepository.GetByIdAsync(categoryDto.Id);
                if (categoryInfo == null)
                {
                    return null;
                }
                var infoUpdate = _mapper.Map<Category>(categoryDto);
                var result = await _categoryRepository.UpdateAsync(infoUpdate);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<bool?> RemoveCategoryDtosAsync(Guid id)
        {
            try
            {
                return await _categoryRepository.DeleteByKey(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<List<CategoryDto>> SearchCategoryDtosAsync(string categoryName)
        {
            try
            {
                var data = await _categoryRepository.SearchCategoryDtosAsync(categoryName);
                return _mapper.Map<List<CategoryDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
