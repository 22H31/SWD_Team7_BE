using BE_Team7.Dtos.CategoryTitle;
using BE_Team7.Dtos.ProductVariant;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IProductVariantRepository
    {
        Task<List<ProductVariant>> GetProductVariantAsync();
        Task<ProductVariant?> GetProductVariantById(Guid variantId);
        Task<ApiResponse<ProductVariant>> CreateProductVariantAsync(ProductVariant productVariant);
        Task<ApiResponse<ProductVariant>> UpdateProductVariantAsync(Guid variantId, UpdateProductVariantRequestDto updateProductVariantRequestDto);
        Task<ApiResponse<ProductVariant>> DeleteProductVariantAsync(Guid variantId);
    }
}
