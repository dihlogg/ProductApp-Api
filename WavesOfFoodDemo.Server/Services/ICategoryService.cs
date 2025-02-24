using WavesOfFoodDemo.Server.Dtos;

namespace WavesOfFoodDemo.Server.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategoryDtosAsync();
        Task<bool> AddCategoryAsync(CategoryCreateDto categoryCreateDto);
        Task<bool?> EditCategoryAsync(CategoryDto categoryDto);
        Task<bool?> RemoveCategoryDtosAsync(Guid id);
        Task<List<CategoryDto>> SearchCategoryDtosAsync(string categoryName);
    }
}
