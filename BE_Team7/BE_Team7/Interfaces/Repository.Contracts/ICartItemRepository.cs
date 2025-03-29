using BE_Team7.Dtos.CartItem;
using BE_Team7.Models;
using System;
using System.Threading.Tasks;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface ICartItemRepository
    {
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem?> GetCartItemByUserAndVariantAsync(string userId, Guid variantId);
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task<bool> UpdateQuantityCartItemAsync(Guid cartItemId, int quantity);
        Task<List<CartItemResponseDto>> GetCartItemsByUserIdAsync(string userId);
        Task<ApiResponse<CartItem>> DeleteCartItemAsync(Guid cartItemId);
        Task<ApiResponse<string>> DeleteMoreCartItemsAsync(List<Guid> cartItemIds);

    }
}
