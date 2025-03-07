using AutoMapper;
using BE_Team7.Dtos.Cart;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CartRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<CartItem>>> GetCartItemsByAccountId(Guid accountId)
        {
            var cart = await _context.Cart
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == accountId);

            if (cart == null || !cart.CartItems.Any())
            {
                return new ApiResponse<List<CartItem>>
                {
                    Success = false,
                    Message = "Giỏ hàng trống.",
                    Data = null
                };
            }

            return new ApiResponse<List<CartItem>>
            {
                Success = true,
                Message = "Lấy giỏ hàng thành công.",
                Data = cart.CartItems
            };
        }

        public async Task<ApiResponse<CartItem>> AddToCartAsync(AddToCartDto addToCartDto)
        {
            var cart = await _context.Cart.FirstOrDefaultAsync(c => c.Id == addToCartDto.Id);
            if (cart == null)
            {
                cart = new Cart { Id = addToCartDto.Id };
                _context.Cart.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = addToCartDto.ProductId,
                Quantity = addToCartDto.Quantity
            };

            _context.CartItem.Add(cartItem);
            await _context.SaveChangesAsync();

            return new ApiResponse<CartItem>
            {
                Success = true,
                Message = "Sản phẩm đã được thêm vào giỏ hàng.",
                Data = cartItem
            };
        }
    }
}
