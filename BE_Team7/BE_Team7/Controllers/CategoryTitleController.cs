using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.CategoryTitle;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Mvc;

namespace BE_Team7.Controllers
{
    [Route("api/categoryTitle")]
    [ApiController]
    public class CategoryTitleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICategoryTitleRepository _categoryTitleRepo;
        public CategoryTitleController(AppDbContext context, ICategoryTitleRepository categoryTitleRepo, IMapper mapper)
        {
            _categoryTitleRepo = categoryTitleRepo;
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategoryTitle()
        {
            var categoryTitles = await _categoryTitleRepo.GetCategoryAsync();
            var categoryTitleDto = _mapper.Map<List<CategoryTitleDto>>(categoryTitles);
            return Ok(categoryTitles);
        }
        [HttpGet("{categoryTitleId:Guid}")]
        public async Task<IActionResult> GetCategoryTitleById([FromRoute] Guid categoryTitleId)
        {
            try
            {
                var categoryTitle = await _categoryTitleRepo.GetCategoryTitleById(categoryTitleId);
                if (categoryTitle == null)
                {
                    return NotFound();
                }
                var categoryTitleDto = _mapper.Map<CategoryTitleDto>(categoryTitle);
                return Ok(categoryTitleDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewCategoryTitle([FromBody] CreateCategoryTitleRequestDto createCategoryTitleRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var categoryTitleModel = _mapper.Map<CategoryTitle>(createCategoryTitleRequestDto);
            await _categoryTitleRepo.CreateCategoryTitleAsync(categoryTitleModel);

            return CreatedAtAction(nameof(GetCategoryTitleById), new { categoryTitleId = categoryTitleModel.CategoryTitleId }, _mapper.Map<CategoryTitleDto>(categoryTitleModel));
        }
        [HttpPut]
        [Route("{categoryTitleId:Guid}")]
        public async Task<IActionResult> UpdateCatregoryTitle([FromRoute] Guid categoryTitleId, [FromBody] UpdateCategoryTitleRequestDto updateCategoryTitleRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Category>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var categoryTitleModel = await _categoryTitleRepo.UpdateCategoryTitleAsync(categoryTitleId, updateCategoryTitleRequestDto);

            if (!categoryTitleModel.Success)
                return NotFound(categoryTitleModel);
            return Ok(categoryTitleModel);
        }
        [HttpDelete("{categoryTitleId:Guid}")]
        public async Task<IActionResult> DeleteCategoryTitle([FromRoute] Guid categoryTitleId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<CategoryTitle>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var categoryTitleModel = await _categoryTitleRepo.DeleteCategoryTitleAsync(categoryTitleId);
            if (!categoryTitleModel.Success)
                return NotFound(categoryTitleModel);
            return Ok(categoryTitleModel);
        }
    }
}
