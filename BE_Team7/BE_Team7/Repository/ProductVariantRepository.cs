using AutoMapper;
using BE_Team7.Dtos.CategoryTitle;
using BE_Team7.Dtos.ProductVariant;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Repository
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ProductVariantRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ProductVariant>> CreateProductVariantAsync(ProductVariant productVariant)
        {
            var productVariantModel = await _context.ProductVariant.FirstOrDefaultAsync(x => x.VariantId == productVariant.VariantId);
            if (productVariantModel != null)
            {
                return new ApiResponse<ProductVariant>
                {
                    Success = false,
                    Message = "Product Variant này đã tồn tại.",
                    Data = null
                };
            }
            _context.ProductVariant.Add(productVariant);
            await _context.SaveChangesAsync();
            return new ApiResponse<ProductVariant>
            {
                Success = true,
                Message = "Tạo sản phẩm thành công.",
                Data = productVariantModel
            };
        }

        public async Task<ApiResponse<ProductVariant>> DeleteProductVariantAsync(Guid variantId)
        {
            var productVariantModel = await _context.ProductVariant.FirstOrDefaultAsync(x => x.VariantId == variantId);
            if (productVariantModel == null)
            {
                return new ApiResponse<ProductVariant>
                {
                    Success = false,
                    Message = "Product Variant không tồn tại",
                    Data = null
                };
            }
            _context.ProductVariant.Remove(productVariantModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<ProductVariant>
            {
                Success = true,
                Message = "Xóa Product Variant thành công",
                Data = productVariantModel
            };
        }

        public async Task<List<ProductVariant>> GetProductVariantAsync()
        {
            var productVariant = _context.ProductVariant.AsQueryable();
            return await productVariant.ToListAsync();
        }

        public async Task<ProductVariant?> GetProductVariantById(Guid variantId)
        {
            return await _context.ProductVariant.FirstOrDefaultAsync(i => i.VariantId == variantId);
        }

        public async Task<ApiResponse<ProductVariant>> UpdateProductVariantAsync(Guid variantId, UpdateProductVariantRequestDto updateProductVariantRequestDto)
        {
            var productVariantModel = await _context.ProductVariant.FirstOrDefaultAsync(x => x.VariantId == variantId);
            if (productVariantModel == null)
            {
                return new ApiResponse<ProductVariant>
                {
                    Success = false,
                    Message = "Product Variant không tồn tại.",
                    Data = null
                };
            }
            _mapper.Map(updateProductVariantRequestDto, productVariantModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<ProductVariant>
            {
                Success = true,
                Message = "Cập nhật Product Variant thành công.",
                Data = productVariantModel
            };
        }
    }
}