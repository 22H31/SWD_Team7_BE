using Microsoft.AspNetCore.Mvc;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Dtos.Cart;
using System;
using System.Threading.Tasks;

namespace BE_Team7.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;

        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            var response = await _cartRepo.AddToCartAsync(addToCartDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{accountId:Guid}")]
        public async Task<IActionResult> GetCart(Guid accountId)
        {
            var response = await _cartRepo.GetCartItemsByAccountId(accountId);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
