using System.Linq;
using BE_Team7.Helpers;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BE_Team7.Repository
{
    public class ProductRepository: IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProductsAsync(ProductQuery productQuery)
        {

            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(productQuery.Name))
            {
                products = products.Where(s => s.Name.Contains(productQuery.Name));
            }

            // phân trang
            var skipNumber = (productQuery.PageNumber - 1) * productQuery.PageSize;

            return await products.Skip(skipNumber).Take(productQuery.PageSize).ToListAsync();
        }
    }
}
