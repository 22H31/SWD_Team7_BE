using BE_Team7.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_Team7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public static List<Product> products = new List<Product>();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(products);
        }

        [HttpPost]
        public IActionResult Create(ProductIf productIf) 
        {
            var product1 = new Product
            {
                ProductId = Guid.NewGuid(),
                Name = productIf.Name,
                Description = productIf.Description,
                CategoryId = productIf.CategoryId,    
                Price = productIf.Price,
                SkinType = productIf.SkinType,
                StockQuantity = productIf.StockQuantity,
                CreatedAt = productIf.CreatedAt
            };
            products.Add(product1);
            return Ok(new
            {
                Success = true, Data = product1
            });
        }
        [HttpGet("{id}")]
        public IActionResult GetByProductId(string id) 
        {
            try
            {
                // LINQ [Object] Query
                var product2 = products.SingleOrDefault(hh => hh.ProductId == Guid.Parse(id));
                if (product2 == null)
                {
                    return NotFound();
                }
                return Ok(product2);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditProduct(string id, Product productEdit) 
        {
            try
            {
                // LINQ [Object] Query
                var product3 = products.SingleOrDefault(hh => hh.ProductId == Guid.Parse(id));
                if (product3 == null)
                {
                    return NotFound();
                }

                if (id != product3.ProductId.ToString()) 
                {
                    return BadRequest();
                }
                // edit
                product3.Name = productEdit.Name;
                product3.Description = productEdit.Description;
                product3.CategoryId = productEdit.CategoryId;
                product3.Price = productEdit.Price;
                product3.SkinType = productEdit.SkinType;
                product3.StockQuantity = productEdit.StockQuantity;
                product3.CreatedAt = productEdit.CreatedAt;
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(string id)
        {
            try
            {
                // LINQ [Object] Query
                var product4 = products.SingleOrDefault(hh => hh.ProductId == Guid.Parse(id));
                if (product4 == null)
                {
                    return NotFound();
                }
                products.Remove(product4);
                return Ok(new
                {
                    Success = true,
                    Data = products.ToArray()
                });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
