using System.Security.Cryptography.Xml;
using BE_Team7.Dtos.Order;
using BE_Team7.Dtos.Refund;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository, AppDbContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }
        //[Authorize(Policy = "RequireUser")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var orderId = await _orderRepository.CreateOrderAsync(request);
                return Ok(new { OrderId = orderId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        //[Authorize(Policy = "RequireUser")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(Guid userId)
        {
            var response = await _orderRepository.GetOrdersByUserIdAsync(userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //[Authorize(Policy = "RequireUser")]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var response = await _orderRepository.GetOrderByIdAsync(orderId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //[Authorize(Policy = "RequireUser")]
        [HttpPut("update-shipping-info/{orderId}/{shippingInfoId}")]
        public async Task<IActionResult> UpdateShippingInfoId(Guid orderId, Guid? shippingInfoId)
        {
            var response = await _orderRepository.UpdateShippingInfoIdAsync(orderId, shippingInfoId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //[Authorize(Policy = "RequireUser")]
        [HttpPut("update-voucher-promotion/{orderId}")]
        public async Task<IActionResult> UpdateVoucherAndPromotion(Guid orderId, [FromBody] UpdateVoucherAndPromotionDto dto)
        {
            var response = await _orderRepository.UpdateVoucherAndPromotionAsync(orderId, dto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //[Authorize(Policy = "RequireUser")]
        [HttpGet("user/{userId}/status/{status}")]
        public async Task<IActionResult> GetOrdersByStatus(Guid userId, string status)
        {
            var response = await _orderRepository.GetOrdersByStatusAsync(userId, status);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        //[Authorize(Policy = "RequireStaffSaleOrAdmin")]
        [HttpGet("manage/status/{status}")]
        public async Task<ActionResult<ApiResponse<List<ManageOrderDto>>>> GetOrdersByStatus(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    return BadRequest(new ApiResponse<List<ManageOrderDto>>
                    {
                        Success = false,
                        Message = "Trạng thái đơn hàng không được để trống.",
                        Data = null
                    });
                }

                var orders = await _orderRepository.GetOrdersByStatusManageAsync(status);

                return Ok(new ApiResponse<List<ManageOrderDto>>
                {
                    Success = true,
                    Message = $"Danh sách đơn hàng với trạng thái: {status}.",
                    Data = orders
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<ManageOrderDto>>
                {
                    Success = false,
                    Message = $"Đã xảy ra lỗi: {ex.Message}",
                    Data = null
                });
            }
        }
        //[Authorize(Policy = "RequireStaffSaleOrAdmin")]
        [HttpPut("manage/{orderId}/comfirmPrepar")]
        public async Task<ActionResult<ApiResponse<string>>> ComfirmPrepar(Guid orderId)
        {
            // Gọi repository để cập nhật trạng thái đơn hàng
            var response = await _orderRepository.UpdateOrderStatusAsync(orderId, "shipping");

            // Trả về phản hồi từ repository
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response);
            }
        }
        //[Authorize(Policy = "RequireStaffSaleOrAdmin")]
        [HttpPut("manage/{orderId}/comfirmShip")]
        public async Task<ActionResult<ApiResponse<string>>> ComfirmShip(Guid orderId)
        {
            // Gọi repository để cập nhật trạng thái đơn hàng
            var response = await _orderRepository.UpdateOrderStatusAsync(orderId, "delivered");

            // Trả về phản hồi từ repository
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response);
            }
        }
        //[Authorize(Policy = "RequireAdmin")]
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            try
            {
                // Tìm order bao gồm các thông tin liên quan để xóa
                var order = await _context.Order
                    .Include(o => o.OrderDetails)
                    .Include(o => o.Payments)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return NotFound($"Order with ID {orderId} not found.");
                }

                // Xóa các bản ghi liên quan trước (nếu cần)
                _context.OrderDetail.RemoveRange(order.OrderDetails);
                _context.Payment.RemoveRange(order.Payments);

                // Xóa order chính
                _context.Order.Remove(order);

                // Lưu thay đổi
                await _context.SaveChangesAsync();

                return NoContent(); // 204 No Content là phản hồi phù hợp khi xóa thành công
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây nếu cần
                return StatusCode(500, $"An error occurred while deleting the order: {ex.Message}");
            }
        }
        //[Authorize(Policy = "RequireStaffSaleOrAdmin")]
        [HttpPost("manage/{orderId}/comfirmRefuning")]
        public async Task<ActionResult<ApiResponse<RefundResponseDto>>> CreateRefund(Guid orderId, [FromBody] CreateRefundDto dto)
        {
            var refundResponse = await _orderRepository.CreateRefundAsync(orderId, dto.Reason);

            if (!refundResponse.Success)
            {
                return BadRequest(new ApiResponse<RefundResponseDto>
                {
                    Success = false,
                    Message = refundResponse.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponse<RefundResponseDto>
            {
                Success = true,
                Message = refundResponse.Message,
                Data = new RefundResponseDto
                {
                    OrderRefundStatus = refundResponse?.Data?.OrderRefundStatus??"Unkonw order Refund Status",
                    RefundId = refundResponse?.Data?.RefundId ?? Guid.Empty,
                }          
            });
        }
        [HttpGet("manage/OrderRefundStatus/{status}")]
        public async Task<ActionResult<ApiResponse<List<ManageOrderDto>>>> GetOrdersByOrderRefundStatus(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    return BadRequest(new ApiResponse<List<ManageOrderDto>>
                    {
                        Success = false,
                        Message = "Trạng thái đơn hàng không được để trống.",
                        Data = null
                    });
                }

                var orders = await _orderRepository.GetOrdersByRefundStatusAsync(status);

                return Ok(new ApiResponse<List<ManageOrderDto>>
                {
                    Success = true,
                    Message = $"Danh sách đơn hàng với trạng thái: {status}.",
                    Data = orders
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<ManageOrderDto>>
                {
                    Success = false,
                    Message = $"Đã xảy ra lỗi: {ex.Message}",
                    Data = null
                });
            }
        }
        [HttpPut("update-customer-info")]
        public async Task<IActionResult> UpdateCustomerRefundInfo([FromBody] CustomerRefundUpdateDto updateDto)
        {
            try
            {
                var updatedRefund = await _orderRepository.UpdateCustomerRefundInfo(updateDto);

                return Ok(new
                {
                    success = true,
                    message = "Cập nhật thông tin hoàn tiền thành công",
                    data = updatedRefund
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi hệ thống khi cập nhật thông tin hoàn tiền",
                    error = ex.Message
                });
            }
        }
    }
}
