using BE_Team7.Helpers;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using BE_Team7.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace BE_Team7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase 
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepo;

        public ProductController(AppDbContext context, IProductRepository productRepo)
        {
            _productRepo = productRepo;
            _context = context;
        }

        // GET: api/stock
        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] ProductQuery query)
        {
            var stocks = await _productRepo.GetProductsAsync(query);
            var stockDto = stocks.ToList();
            return Ok(stockDto);
        }

    }


}
