using BE_Team7.Dtos.CartItem;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _context;

        public CartItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            _context.CartItem.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem?> GetCartItemByUserAndVariantAsync(string userId, Guid variantId)
        {
            return await _context.CartItem
                .FirstOrDefaultAsync(c => c.Id == userId && c.VariantId == variantId);
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItem.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> UpdateQuantityCartItemAsync(Guid cartItemId, int quantity)
        {
            var cartItem = await _context.CartItem.FirstOrDefaultAsync(c => c.CartItemId == cartItemId);

            if (cartItem == null)
            {
                Console.WriteLine($"❌ CartItemId {cartItemId} không tồn tại trong DB.");
                return false;
            }

            Console.WriteLine($"✅ CartItemId {cartItemId} tồn tại. Đang cập nhật số lượng...");
            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CartItemResponseDto>> GetCartItemsByUserIdAsync(string userId)
        {
            return await _context.CartItem
                .Where(ci => ci.Id == userId)
                .OrderByDescending(ci => ci.CartItemCreateAt)
                .Select(ci => new CartItemResponseDto
                {
                    VariantId = ci.VariantId,
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductVariant.ProductId,
                    ProductAvatarImage = ci.ProductVariant.Product.ProductAvatarImages.Select(img => img.ImageUrl)
                    .FirstOrDefault()!,
                    ProductName = ci.ProductVariant.Product.ProductName,
                    Quantity = ci.Quantity,
                    Price = ci.ProductVariant.Price,
                    SkinType = ci.ProductVariant.SkinType
                })
                .ToListAsync();
        }

        public async Task<ApiResponse<CartItem>> DeleteCartItemAsync(Guid cartItemId)
        {    
            var cartItemModel = await _context.CartItem.FirstOrDefaultAsync(x => x.CartItemId == cartItemId);
            if (cartItemModel == null)
            {
                return new ApiResponse<CartItem>
                {
                    Success = false,
                    Message = "CartItem không tồn tại.",
                    Data = null
                };
            }
            _context.CartItem.Remove(cartItemModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<CartItem>
            {
                Success = true,
                Message = "Xóa CartItem thành công",
                Data = cartItemModel
            };
        }
        public async Task<ApiResponse<string>> DeleteMoreCartItemsAsync(List<Guid> cartItemIds)
        {
            var cartItems = await _context.CartItem
                                          .Where(x => cartItemIds.Contains(x.CartItemId))
                                          .ToListAsync();

            if (cartItems.Count == 0)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Không tìm thấy CartItem nào để xóa.",
                    Data = null
                };
            }

            _context.CartItem.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = $"Đã xóa {cartItems.Count} CartItem thành công.",
                Data = null
            };
        }
    }
}
