using BE_Team7.Helpers;
using BE_Team7.Interfaces.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using BE_Team7.Dtos.Product;
using BE_Team7.Dtos.ProductVariant;
using AutoMapper;
using BE_Team7.Shared.Extensions;
using BE_Team7.Interfaces.Service.Contracts;
using GarageManagementAPI.Shared.ResultModel;
using BE_Team7.Shared.ErrorModel;
using System.Text.Json;
using BE_Team7.Repository;

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
                SoldQuantity = p.SoldQuantity,
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                BrandName = p.Brand?.BrandName,
                CategoryName = p.Category?.CategoryName,
                AverageRating = p.Feedbacks.Any() ? p.Feedbacks.Average(f => f.Rating) : 0,
                TotalFeedback = p.Feedbacks.Count,
                AvartarImageUrl = p.ProductAvatarImages
                .OrderByDescending(img => img.ProductAvatarImageCreatedAt)
                .Select(img => img.ImageUrl)
                .FirstOrDefault(),
                Variants = p.Variants.Select(v => new ProductVariantDto
                {
                    VariantId = v.VariantId,
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
        [HttpGet("category-title-id/{categoryTitleId}")]
        public async Task<IActionResult> GetProductsByCategoryTitleId(Guid categoryTitleId, [FromQuery] ProductQuery query)
        {
            var pagedResult = await _productRepo.GetProductsByCategoryTitleIdAsync(categoryTitleId, query);
            return Ok(MapPagedResult(pagedResult, query));
        }
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(Guid categoryId, [FromQuery] ProductQuery query)
        {
            var pagedResult = await _productRepo.GetProductsByCategoryAsync(categoryId, query);
            return Ok(MapPagedResult(pagedResult, query));
        }

        [HttpGet("brand/{brandId}")]
        public async Task<IActionResult> GetProductsByBrand(Guid brandId, [FromQuery] ProductQuery query)
        {
            var pagedResult = await _productRepo.GetProductsByBrandAsync(brandId, query);
            return Ok(MapPagedResult(pagedResult, query));
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentProducts([FromQuery] ProductQuery query)
        {
            var pagedResult = await _productRepo.GetRecentProductsAsync(query);
            return Ok(MapPagedResult(pagedResult, query));
        }

        [HttpGet("top-selling")]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] ProductQuery query)
        {
            var pagedResult = await _productRepo.GetTopSellingProductsAsync(query);
            return Ok(MapPagedResult(pagedResult, query));
        }

        //[Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto createProductRequestDto)
        {
            if (createProductRequestDto == null)
                return BadRequest("Invalid product data.");

            if (createProductRequestDto.Describe == null ||
                createProductRequestDto.Specifications == null ||
                createProductRequestDto.UseManual == null)
            {
                return BadRequest("Incomplete product details.");
            }

            var product = new Product
            {
                SoldQuantity = 0,
                ProductId = Guid.NewGuid(), // Tạo ID mới
                ProductName = createProductRequestDto.ProductName,
                BrandId = createProductRequestDto.BrandId,
                CategoryId = createProductRequestDto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                Description = JsonSerializer.Serialize(createProductRequestDto.Describe),
                Specification = JsonSerializer.Serialize(createProductRequestDto.Specifications),
                UseManual = JsonSerializer.Serialize(createProductRequestDto.UseManual)
            };

            // Lưu vào database
            Console.WriteLine(JsonSerializer.Serialize(product));
            await _productRepo.CreateProductAsyns(product);
            // Kiểm tra lại dữ liệu ngay sau khi lưu
            var savedProduct = await _context.Products.FindAsync(product.ProductId);
            if (savedProduct == null)
            {
                Console.WriteLine("Dữ liệu chưa được lưu vào DB!");
            }
            else
            {
                Console.WriteLine("Dữ liệu đã được lưu vào DB thành công!");
            }

            // Trả về ProductId cho FE
            return Ok(new { productId = product.ProductId });
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            var product = await _productRepo.GetProductById(productId);

            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            return Ok(product);
        }
        [HttpPut]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid productId, [FromBody] UpdateProductRequestDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Product>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var productModel = await _productRepo.UpdateProductById(productId, updateDto);
            if (!productModel.Success)
                return NotFound(productModel);
            return Ok(productModel);
        }
        [HttpDelete]
        [Route("{productId:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid productId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Product>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var productModel = await _productRepo.DeleteProductById(productId);

            if (!productModel.Success)
                return NotFound(productModel);
            return Ok(productModel);
        }
        [HttpPost("{productId:guid}/product_images", Name = "CreateProductImage")]
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
        [HttpPost("{productId:guid}/product_avartar_images", Name = "CreateProductAvartarImage")]
        public async Task<IActionResult> CreateProductAvartarImage(Guid productId, [FromForm] List<IFormFile> fileDtos)
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
            var createdProductAvartarImages = new List<object>();
            foreach (var fileDto in fileDtos)
            {
                var uploadFileResult = await _mediaService.UploadProductAvatarImageAsync(fileDto);

                if (!uploadFileResult.IsSuccess)
                    return BadRequest(Result.Failure(HttpStatusCode.BadRequest, uploadFileResult.Errors));

                var imgTuple = uploadFileResult.GetValue<(string? publicId, string? absoluteUrl)>();

                var updateResult = await _productRepo.CreateProductAvartarImgAsync(productId, imgTuple.publicId!, imgTuple.absoluteUrl!);

                if (!updateResult.Success) return BadRequest(Result.Failure(HttpStatusCode.BadRequest, new List<ErrorsResult>()));

            }
            return Ok();
        }
        private object MapPagedResult(PagedResult<Product> pagedResult, ProductQuery query)
        {
            var productDtos = pagedResult.Items.Select(p => new ProductResponseDto
            {
                SoldQuantity = p.SoldQuantity,
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                BrandName = p.Brand?.BrandName,
                CategoryName = p.Category?.CategoryName,
                AverageRating = p.Feedbacks.Any() ? p.Feedbacks.Average(f => f.Rating) : 0,
                TotalFeedback = p.Feedbacks.Count,
                AvartarImageUrl = p.ProductAvatarImages
                    .OrderByDescending(img => img.ProductAvatarImageCreatedAt)
                    .Select(img => img.ImageUrl)
                    .FirstOrDefault(),
                Variants = p.Variants.Select(v => new ProductVariantDto
                {
                    VariantId = v.VariantId,
                    Volume = v.Volume,
                    SkinType = v.SkinType,
                    Price = v.Price,
                    StockQuantity = v.StockQuantity
                }).ToList()
            }).ToList();

            return new
            {
                TotalCount = pagedResult.TotalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                Items = productDtos
            };
        }
    }
}