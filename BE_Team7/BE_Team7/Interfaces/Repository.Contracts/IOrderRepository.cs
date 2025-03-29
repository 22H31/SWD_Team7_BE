using BE_Team7.Dtos.Order;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IOrderRepository
    {
        Task<Guid> CreateOrderAsync(CreateOrderRequest request);
        Task<ApiResponse<List<OrderDto>>> GetOrdersByUserIdAsync(Guid userId);
        Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(Guid orderId);
        Task<ApiResponse<bool>> UpdateShippingInfoIdAsync(Guid orderId, Guid? shippingInfoId);
        Task<ApiResponse<bool>> UpdateVoucherAndPromotionAsync(Guid orderId, UpdateVoucherAndPromotionDto dto);
        Task<ApiResponse<List<OrderDto>>> GetOrdersByStatusAsync(Guid userId, string status);
        Task<List<ManageOrderDto>> GetOrdersByStatusManageAsync(string status);
        Task<ApiResponse<string>> UpdateOrderStatusAsync(Guid orderId, string status);
        Task<ApiResponse<OrderRefund>> CreateRefundAsync(Guid orderId, string reason);
        Task<OrderRefund> UpdateCustomerRefundInfo(CustomerRefundUpdateDto updateDto);
        Task<List<ManageOrderDto>> GetOrdersByRefundStatusAsync(string refundStatus);
    }
}
