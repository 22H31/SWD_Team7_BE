using BE_Team7.Dtos.Product;
using BE_Team7.Helpers;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync(ProductQuery productQuery);
        Task<Product> CreateProductAsyns(Product product);
        Task<Product?> GetProductById(Guid productId);
        Task<ApiResponse<Product>> UpdateProductById(Guid id, UpdateProductRequestDto product);
    }
}
