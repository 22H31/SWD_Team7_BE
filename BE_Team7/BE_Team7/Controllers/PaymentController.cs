using BE_Team7.Interfaces.Service.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService, AppDbContext appDbContext)
        {
            _vnPayService = vnPayService;
            _context = appDbContext;
        }
        //[Authorize(Policy = "RequireUser")]
        [HttpGet("vnPay")]
        public async Task<IActionResult> PaymentCallbackVnPay()
        {
            try
            {
                var response = _vnPayService.PaymentExecute(Request.Query);
                var order = await _context.Order
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductVariant) // Include ProductVariant để truy xuất ProductId
                    .FirstOrDefaultAsync(o => o.OrderId == response.OrderId);

                Console.WriteLine("quang.............."+response.OrderId);

                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                if (response.VnPayResponseCode == "00")
                {
                    // Thanh toán thành công
                    order.OrderStatus = "preparing";

                    // Trừ số lượng voucher
                    if (order.VoucherId != null)
                    {
                        var voucher = await _context.Voucher.FirstOrDefaultAsync(v => v.VoucherId == order.VoucherId);
                        if (voucher != null && voucher.VoucherQuantity > 0)
                        {
                            voucher.VoucherQuantity -= 1;
                        }
                    }

                    // Trừ số lượng sản phẩm trong kho & cập nhật SoldQuantity
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        var productVariant = await _context.ProductVariant
                            .FirstOrDefaultAsync(pv => pv.VariantId == orderDetail.VariantId);

                        if (productVariant != null && productVariant.StockQuantity >= orderDetail.Quantity)
                        {
                            productVariant.StockQuantity -= orderDetail.Quantity;

                            // Tìm Product và cập nhật SoldQuantity
                            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productVariant.ProductId);
                            if (product != null)
                            {
                                product.SoldQuantity += orderDetail.Quantity;
                            }
                        }
                        else
                        {
                            return BadRequest(new { message = "Not enough stock for product variant: " + orderDetail.VariantId });
                        }
                    }

                    // Lưu thông tin thanh toán vào bảng Payments
                    var payment = new Payments
                    {
                        PaymentId = response.PaymentId,
                        OrderId = response.OrderId,
                        OrderDescription = response.OrderDescription,
                        TransactionId = response.TransactionId,
                        PaymentMethod = response.PaymentMethod,
                        Success = response.Success,
                        Token = response.Token,
                        VnPayResponseCode = response.VnPayResponseCode
                    };

                    _context.Payment.Add(payment);
                    await _context.SaveChangesAsync();
                    return Json(new { status = "success", message = "Payment successful" });
                }
                else
                {
                    order.VoucherId = null;
                    order.PromotionId = null;
                    order.VoucherFee = 0;
                    order.PromotionFee = 0;
                    order.ShippingFee = 0;
                    order.FinalAmount = order.TotalAmount;

                    await _context.SaveChangesAsync();
                    Console.WriteLine("chạy tới tận đây nè");
                    return Json(new { status = "fail", message = "Payment failed" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }
        //[Authorize(Policy = "RequireUser")]
        [HttpPost("{orderId}/pay")]
        public async Task<IActionResult> PayOrder(Guid orderId)
        {
            try
            {
                // Lấy thông tin Order từ database
                var order = await _context.Order
                    .Include(o => o.User)
                    .Include(o => o.ShippingInfo)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                // Kiểm tra trạng thái đơn hàng
                if (order.OrderStatus?.ToLower() != "paying")
                {
                    return BadRequest(new
                    {
                        message = "Cannot process payment for this order.",
                        details = $"Order status must be 'paying' but current status is '{order.OrderStatus}'."
                    });
                }

                // Kiểm tra thông tin vận chuyển
                if (order.ShippingInfoId == null || order.ShippingInfo == null)
                {
                    return BadRequest(new
                    {
                        message = "Cannot process payment for this order.",
                        details = "Shipping information is required."
                    });
                }

                // Tạo thông tin thanh toán
                var paymentInfo = new PaymentInformation
                {
                    OrderType = "Thanh toán đơn hàng",
                    Amount = (double)order.FinalAmount,
                    OrderDescription = $"Thanh toán đơn hàng #{order.OrderId}",
                    Name = order.User?.Name ?? "Unknow Name"
                };

                // Tạo URL thanh toán VNPay
                var paymentUrl = _vnPayService.CreatePaymentUrl(paymentInfo, HttpContext, orderId);

                return Ok(new { paymentUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while processing your request.",
                    error = ex.Message
                });
            }
        }
    }
}
