using Microsoft.AspNetCore.Mvc;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Services;

namespace WavesOfFoodDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(
            ILogger<CategoryController> logger,
            ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategoryInfos()
        {
            try
            {
                var data = await _categoryService.GetCategoryDtosAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SearchCategory")]
        public async Task<IActionResult> SearchCategory(string categoryName)
        {
            try
            {
                var data = await _categoryService.SearchCategoryDtosAsync(categoryName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostCategory")]
        public async Task<IActionResult> PostCategory(CategoryCreateDto categoryCreateDto)
        {
            try
            {
                var data = await _categoryService.AddCategoryAsync(categoryCreateDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PutCategory")]
        public async Task<IActionResult> PutCategory(CategoryDto categoryDto)
        {
            try
            {
                var data = await _categoryService.EditCategoryAsync(categoryDto);
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

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var data = await _categoryService.RemoveCategoryDtosAsync(id);
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