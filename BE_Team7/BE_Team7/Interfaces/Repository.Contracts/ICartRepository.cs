using BE_Team7.Dtos.Cart;
using BE_Team7.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface ICartRepository
    {
        Task<ApiResponse<List<CartItem>>> GetCartItemsByAccountId(Guid accountId);
        Task<ApiResponse<CartItem>> AddToCartAsync(AddToCartDto addToCartDto);
    }
}
