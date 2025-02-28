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
        public async Task<List<Product>> GetProductsAsync(ProductQuery productQuery)
        {

            var products = _context.Products.Include(c => c.Category).Include(b => b.Brand).AsQueryable();

            if (!string.IsNullOrWhiteSpace(productQuery.Name))
            {
                products = products.Where(s => s.ProductName.Contains(productQuery.Name));
            }

            // phân trang
            var skipNumber = (productQuery.PageNumber - 1) * productQuery.PageSize;

            return await products.Skip(skipNumber).Take(productQuery.PageSize).ToListAsync();
        }

        public async Task<Product> CreateProductAsyns(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> GetProductById(Guid productId)
        {
            return await _context.Products.FirstOrDefaultAsync(i => i.ProductId == productId);
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
    }
}
