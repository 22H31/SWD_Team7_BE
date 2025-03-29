using BE_Team7.Dtos.CartItem;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Controllers
{
    [Route("api/cartitem")]
    [ApiController]
    public class CartItemController : Controller
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly AppDbContext _context;

        public CartItemController(ICartItemRepository cartItemRepository, AppDbContext context)
        {
            _cartItemRepository = cartItemRepository;
            _context = context;
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpPost("add")]
        public async Task<IActionResult> AddCartItem([FromBody] CreateCartItemDto createCartItemDto)
        {
            if (createCartItemDto == null || createCartItemDto.Quantity <= 0)
            {
                return BadRequest("Invalid request data.");
            }

            if (!Guid.TryParse(createCartItemDto.VariantId, out Guid variantGuid))
            {
                return BadRequest("Invalid VariantId format.");
            }
            var userExists = await _context.Users.AnyAsync(u => u.Id == createCartItemDto.UserId);
            if (!userExists)
            {
                return BadRequest("User does not exist.");
            }
            // Kiểm tra xem CartItem có tồn tại không
            var existingCartItem = await _cartItemRepository.GetCartItemByUserAndVariantAsync(createCartItemDto.UserId, variantGuid);

            if (existingCartItem != null)
            {
                // Nếu tồn tại, cập nhật số lượng và thời gian
                existingCartItem.Quantity += createCartItemDto.Quantity;
                existingCartItem.CartItemCreateAt = DateTime.UtcNow;
                var updatedCartItem = await _cartItemRepository.UpdateCartItemAsync(existingCartItem);
                return Ok(new { Message = "CartItem updated successfully", Data = updatedCartItem });
            }

            // Nếu chưa có, tạo mới
            var cartItem = new CartItem
            {
                CartItemId = Guid.NewGuid(),
                Id = createCartItemDto.UserId,
                VariantId = variantGuid,
                Quantity = createCartItemDto.Quantity,
                CartItemCreateAt = DateTime.UtcNow,
            };

            var createdCartItem = await _cartItemRepository.AddCartItemAsync(cartItem);

            return Ok(new { Message = "CartItem added successfully", Data = createdCartItem });
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpPut("update-cartitem/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem([FromRoute] Guid cartItemId, [FromBody] UpdateCartItemDto updateCartItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = await _cartItemRepository.UpdateQuantityCartItemAsync(cartItemId, updateCartItemDto.Quantity);
            if (!isUpdated)
            {
                return NotFound("Không tìm thấy sản phẩm trong giỏ hàng.");
            }
            return Ok(new { message = "Cập nhật số lượng thành công." });
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetCartItemsByUser(string id)
        {
            var cartItems = await _cartItemRepository.GetCartItemsByUserIdAsync(id);
            if (cartItems == null || !cartItems.Any())
            {
                return NotFound("Không tìm thấy sản phẩm trong giỏ hàng.");
            }
            return Ok(cartItems);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] Guid cartItemId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<CartItem>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var cartItemModel = await _cartItemRepository.DeleteCartItemAsync(cartItemId);
            if (!cartItemModel.Success)
                return NotFound(cartItemModel);
            return Ok(cartItemModel);
        }
        [HttpDelete("bulk-delete")]
        public async Task<IActionResult> DeletemoreCartItems([FromBody] List<Guid> cartItemIds)
        {
            if (cartItemIds == null || !cartItemIds.Any())
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Danh sách cartItemId không hợp lệ.",
                    Data = null
                });
            }

            var result = await _cartItemRepository.DeleteMoreCartItemsAsync(cartItemIds);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

    }
}
