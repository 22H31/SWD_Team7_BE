using BE_Team7.Helpers;
using BE_Team7.Interfaces.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using BE_Team7.Dtos.Product;
using AutoMapper;
using BE_Team7.Shared.Extensions;
using BE_Team7.Interfaces.Service.Contracts;
using GarageManagementAPI.Shared.ResultModel;
using BE_Team7.Shared.ErrorModel;

namespace BE_Team7.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepo;
        private readonly IMediaService _mediaService;

        public ProductController(AppDbContext context, IProductRepository productRepo, IMapper mapper, IMediaService mediaService)
        {
            _productRepo = productRepo;
            _context = context;
            _mapper = mapper;
            _mediaService = mediaService;
        }

        // GET: api/product
        //[Authorize(Policy = "RequireUser")]
        //[HttpGet]


        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQuery query)
        {
            var pagedResult = await _productRepo.GetProductsAsync(query);

            var productDtos = pagedResult.Items.Select(p => new ProductResponseDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                BrandName = p.Brand?.BrandName,
                CategoryName = p.Category?.CategoryName,
                AverageRating = p.Feedbacks.Any() ? p.Feedbacks.Average(f => f.Rating) : 0,
                TotalFeedback = p.Feedbacks.Count,
                Variants = p.Variants.Select(v => new ProductVariantDto
                {
                    Volume = v.Volume,
                    SkinType = v.SkinType,
                    Price = v.Price,
                    StockQuantity = v.StockQuantity
                }).ToList()
            }).ToList();

            return Ok(new
            {
                TotalCount = pagedResult.TotalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                Items = productDtos
            });
        }

        //[Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateNewProduct([FromBody] CreateProductRequestDto createProductRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var productModel = _mapper.Map<Product>(createProductRequestDto);
            var response = await _productRepo.CreateProductAsyns(productModel);
            // lỗi từ Repository
            if (!response.Success)
            {
                return BadRequest(new { message = response.Message });
            }

            return CreatedAtAction(nameof(GetProductDetailById), new { id = productModel.ProductId }, _mapper.Map<ProductDetailDto>(productModel));
        }
        [HttpGet("{productId:Guid}")]
        public async Task<IActionResult> GetProductDetailById(Guid productId)
        {
            var product = await _productRepo.GetProductById(productId);
            if (product == null)
            {
                return NotFound(new { message = "Sản phẩm không tồn tại." });
            }

            var productDto = _mapper.Map<ProductDetailDto>(product);
            return Ok(productDto);
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
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Product>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var productModel = await _productRepo.DeleteProductById(id);

            if (!productModel.Success)
                return NotFound(productModel);
            return Ok(productModel);
        }
        [HttpPost("{productId:guid}/images", Name = "CreateProductImage")]
        public async Task<IActionResult> CreateProductImage(Guid productId, [FromForm] List<IFormFile> fileDtos)
        {
            var productExists = await _productRepo.GetProductById(productId);
            if (productExists == null)
            {
                return NotFound(new { message = "Sản phẩm không tồn tại." });
            }
            if (fileDtos == null || !fileDtos.Any())
            {
                return BadRequest("No files were uploaded.");
            }
            var createdProductImages = new List<object>();
            foreach (var fileDto in fileDtos)
            {
                var uploadFileResult = await _mediaService.UploadProductImageAsync(fileDto);

                if (!uploadFileResult.IsSuccess)
                    return BadRequest(Result.Failure(HttpStatusCode.BadRequest, uploadFileResult.Errors));

                var imgTuple = uploadFileResult.GetValue<(string? publicId, string? absoluteUrl)>();

                var updateResult = await _productRepo.CreateProductImgAsync(productId, imgTuple.publicId!, imgTuple.absoluteUrl!);

                if (!updateResult.Success) return BadRequest(Result.Failure(HttpStatusCode.BadRequest, new List<ErrorsResult>()));

            }
            return Ok();
        }
    }
}
