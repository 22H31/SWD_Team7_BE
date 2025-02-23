using BE_Team7.Helpers;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using BE_Team7.Mappers;
using Microsoft.AspNetCore.Mvc;
using BE_Team7.Dtos.Product;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;

namespace BE_Team7.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepo;

        public ProductController(AppDbContext context, IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/product
        [Authorize(Policy = "RequireUser")]
        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] ProductQuery query)
        {
            var products = await _productRepo.GetProductsAsync(query);
            var productdDto = _mapper.Map<List<ProductDto>>(products);
            return Ok(productdDto);
        }
        [Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequestDto createProductRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var productModel = _mapper.Map<Product>(createProductRequestDto);
            await _productRepo.CreateProductAsyns(productModel);

            return CreatedAtAction(nameof(GetById), new { id = productModel.ProductId }, _mapper.Map<ProductDto>(productModel));
        }
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var product = await _productRepo.GetProductById(id);
                if (product == null)
                {
                    return NotFound();
                }
                var productDto = _mapper.Map<ProductDto>(product);
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductRequestDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Product>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var productModel = await _productRepo.UpdateProductById(id, updateDto);

            if (!productModel.Success)
                return NotFound(productModel);
            return Ok(productModel);
        }

    }
}
