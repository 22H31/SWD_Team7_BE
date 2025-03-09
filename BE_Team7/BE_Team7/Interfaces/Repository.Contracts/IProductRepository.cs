using System.Runtime.CompilerServices;
using BE_Team7.Dtos.Product;
using BE_Team7.Helpers;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> GetProductsAsync(ProductQuery productQuery);
        Task<ApiResponse<Product>> CreateProductAsyns(Product product);
        Task<Product?> GetProductById(Guid productId);
        Task<ApiResponse<Product>> UpdateProductById(Guid id, UpdateProductRequestDto product);
        Task<ApiResponse<Product>> DeleteProductById(Guid id);
        Task<ApiResponse<Product>> CreateProductImgAsync(Guid productId, string publicId, string absoluteUrl);
    }
}
