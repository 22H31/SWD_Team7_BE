using System.Linq;
using AutoMapper;
using BE_Team7.Dtos.Product;
using BE_Team7.Helpers;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;
using static StackExchange.Redis.Role;

namespace BE_Team7.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PagedResult<Product>> GetProductsAsync(ProductQuery productQuery)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Feedbacks)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(productQuery.Name))
            {
                products = products.Where(p => p.ProductName.Contains(productQuery.Name));
            }
            var totalCount = await products.CountAsync();
            var skipNumber = (productQuery.PageNumber - 1) * productQuery.PageSize;
            var pagedProducts = await products.Skip(skipNumber).Take(productQuery.PageSize).ToListAsync();

            return new PagedResult<Product>
            {
                Items = pagedProducts,
                TotalCount = totalCount
            };
        }


        public async Task<ApiResponse<Product>> CreateProductAsyns(Product product)
        {
            // Kiểm tra Category có tồn tại không
            var categoryExists = await _context.Category.AnyAsync(c => c.CategoryId == product.CategoryId);
            if (!categoryExists)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Danh mục không tồn tại.",
                    Data = null
                };
            }
            // Kiểm tra Brand có tồn tại không
            var brandExists = await _context.Brand.AnyAsync(b => b.BrandId == product.BrandId);
            if (!brandExists)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Thương hiệu không tồn tại.",
                    Data = null
                };
            }
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Tên sản phẩm không được để trống.",
                    Data = null
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Tạo sản phẩm thành công.",
                Data = product
            };
        }

        public async Task<Product?> GetProductById(Guid productId)
        {
            return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .Include(p => p.Variants)
            .Include(p => p.Feedbacks)
            .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<ApiResponse<Product>> UpdateProductById(Guid id, UpdateProductRequestDto productDtoForUpdate)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (productModel == null)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Sản phẩm không tồn tại.",
                    Data = null
                };
            }

            if (productModel.CategoryId != productDtoForUpdate.CategoryId)
            {
                var categoryExists = await _context.Category.AnyAsync(c => c.CategoryId == productDtoForUpdate.CategoryId);
                if (!categoryExists)
                {
                    return new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "Danh mục không tồn tại.",
                        Data = null
                    };
                }
                productModel.CategoryId = productDtoForUpdate.CategoryId;
            }
            if (productModel.BrandId != productDtoForUpdate.BrandId)
            {
                var brandExists = await _context.Brand.AnyAsync(c => c.BrandId == productDtoForUpdate.BrandId);
                if (!brandExists)
                {
                    return new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "Brand không tồn tại.",
                        Data = null
                    };
                }
                productModel.BrandId = productDtoForUpdate.BrandId;
            }

            _mapper.Map(productDtoForUpdate, productModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Cập nhật sản phẩm thành công.",
                Data = productModel
            };
        }

        public async Task<ApiResponse<Product>> DeleteProductById(Guid id)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if (productModel == null)
            {
                return new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Sản phẩm không tồn tại.",
                    Data = null
                };
            }
            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Xóa sản phẩm thành công.",
                Data = productModel
            };
        }
        public async Task<ApiResponse<Product>> CreateProductImgAsync(Guid productId, string publicId, string absoluteUrl)
        {

            var productImg = new ProductImage()
            {
                ImageUrl = absoluteUrl,
                ImageId = publicId,
                ProductId = productId
            };
            _context.ProductImage.Add(productImg);
            await _context.SaveChangesAsync();

            return new ApiResponse<Product>
            {
                Success = true,
                Message = "Cập nhật sản phẩm thành công.",
                Data = null,
            };
        }
    }
}
