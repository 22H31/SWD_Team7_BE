using AutoMapper;
using BE_Team7.Dtos.CategoryTitle;
using BE_Team7.Dtos.ProductVariant;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Team7.Controllers
{
    [Route("api/productVariant")]
    [ApiController]
    public class ProductVariantController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductVariantRepository _productVariantRepo;
        public ProductVariantController(AppDbContext context, IProductVariantRepository productVariantRepo, IMapper mapper)
        {
            _productVariantRepo = productVariantRepo;
            _context = context;
            _mapper = mapper;
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet]
        public async Task<IActionResult> GetAllProductVariant()
        {
            var productVariants = await _productVariantRepo.GetProductVariantAsync();
            var productVariantDto = _mapper.Map<List<ProductVariantDto>>(productVariants);
            return Ok(productVariants);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("{variantId}")]
        public async Task<IActionResult> GetProductVariantById([FromRoute] string variantId)
        {
            try
            {
                var productVariant = await _productVariantRepo.GetProductVariantById(variantId);
                if (productVariant == null)
                {
                    return NotFound();
                }
                var productVariantDto = _mapper.Map<ProductVariantDto>(productVariant);
                return Ok(productVariantDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        //[Authorize(Policy = "RequireStaffSaleOrStaff")]
        [HttpPost]
        public async Task<IActionResult> CreateNewProductVariant([FromBody] CreateProductVariantRequestDto createProductVariantRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var productVariantModel = _mapper.Map<ProductVariant>(createProductVariantRequestDto);
            await _productVariantRepo.CreateProductVariantAsync(productVariantModel);
            return CreatedAtAction(nameof(GetProductVariantById), new { variantId = productVariantModel.VariantId }, _mapper.Map<ProductVariantDto>(productVariantModel));
        }
        //[Authorize(Policy = "RequireStaffSaleOrStaff")]
        [HttpPut]
        [Route("{variantId:Guid}")]
        public async Task<IActionResult> UpdateProductVariant([FromRoute] Guid variantId, [FromBody] UpdateProductVariantRequestDto updateProductVariantRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<ProductVariant>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var productVariantModel = await _productVariantRepo.UpdateProductVariantAsync(variantId, updateProductVariantRequestDto);

            if (!productVariantModel.Success)
                return NotFound(productVariantModel);
            return Ok(productVariantModel);
        }
        //[Authorize(Policy = "RequireAlllStaff")]
        [HttpDelete("{variantId:Guid}")]
        public async Task<IActionResult> DeleteProductVariant([FromRoute] Guid variantId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<ProductVariant>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var productVariantModel = await _productVariantRepo.DeleteProductVariantAsync(variantId);
            if (!productVariantModel.Success)
                return NotFound(productVariantModel);
            return Ok(productVariantModel);
        }
    }
}