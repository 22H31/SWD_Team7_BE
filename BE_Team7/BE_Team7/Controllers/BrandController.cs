using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.Product;
using BE_Team7.Helpers;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Mvc;

namespace BE_Team7.Controllers
{
    [Route("api/brand")]
    [ApiController]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepo;
        public BrandController(AppDbContext context, IBrandRepository brandRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBrand()
        {
            var brands = await _brandRepo.GetBrandAsync();  
            var brandDto = _mapper.Map<List<BrandDto>>(brands);
            return Ok(brandDto);
        }
        [HttpGet("{brandId}")]
        public async Task<IActionResult> GetBrandById([FromRoute] string brandId)
        {
            try
            {
                var brand = await _brandRepo.GetBrandById(brandId);
                if (brand == null)
                {
                    return NotFound();
                }
                var brandDto = _mapper.Map<BrandDto>(brand);
                return Ok(brandDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewBrand([FromBody] CreateBrandRequestDto createBrandRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var brandModel = _mapper.Map<Brand>(createBrandRequestDto);
            await _brandRepo.CreateBrandAsync(brandModel);

            return CreatedAtAction(nameof(GetBrandById), new { brandId = brandModel.BrandId }, _mapper.Map<BrandDto>(brandModel));
        }
        [HttpPut]
        [Route("{brandId:Guid}")]
        public async Task<IActionResult> UpdateBrand([FromRoute] Guid brandId, [FromBody] UpdateBrandRequestDto updateBrandRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Brand>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var brandModel = await _brandRepo.UpdateBrandAsync(brandId, updateBrandRequestDto);

            if (!brandModel.Success)
                return NotFound(brandModel);
            return Ok(brandModel);
        }
        [HttpDelete("{brandId:Guid}")]
        public async Task<IActionResult> DeleteBrand([FromRoute] Guid brandId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Brand>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var brandModel = await _brandRepo.DeleteBrandAsync(brandId);
            if (!brandModel.Success)
                return NotFound(brandModel);
            return Ok(brandModel);
        }
    }
}
