using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.Category;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Team7.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(AppDbContext context, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepo = categoryRepository;
            _context = context;
            _mapper = mapper;
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepo.GetAllCategoriesAsync();
            return Ok(categories);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("{categoryId:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId)
        {
            try
            {
                var category = await _categoryRepo.GetCategoryById(categoryId);
                if (category == null)
                {
                    return NotFound();
                }
                var categoryDto = _mapper.Map<CategoryDto>(category);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        //[Authorize(Policy = "RequireStaffSale")]
        [HttpPost]
        public async Task<IActionResult> CreateNewCategory([FromBody] CreateCategoryRequestDto createCategoryRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var categoryModel = _mapper.Map<Category>(createCategoryRequestDto);
            await _categoryRepo.CreateCategoryAsync(categoryModel);

            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = categoryModel.CategoryId }, _mapper.Map<CategoryDto>(categoryModel));

        }
        //[Authorize(Policy = "RequireStaffSale")]
        [HttpPut]
        [Route("{categoryId:Guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromBody] UpdateCategoryRequestDto updateCategoryRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Category>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var categoryModel = await _categoryRepo.UpdateCategoryAsync(categoryId, updateCategoryRequestDto);

            if (!categoryModel.Success)
                return NotFound(categoryModel);
            return Ok(categoryModel);
        }
        //[Authorize(Policy = "RequireStaffSaleOrAdmin")]
        [HttpDelete("{categoryId:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid categoryId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Category>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var categoryModel = await _categoryRepo.DeleteCategoryAsync(categoryId);
            if (!categoryModel.Success)
                return NotFound(categoryModel);
            return Ok(categoryModel);
        }

    }
}
