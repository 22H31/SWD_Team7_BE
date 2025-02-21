using BE_Team7.Helpers;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync(ProductQuery productQuery);
    }
}
