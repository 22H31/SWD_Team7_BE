using BE_Team7.Dtos.Order;
using BE_Team7.Models;
using BE_Team7.Interfaces.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using BE_Team7.Dtos.ShippingInfo;
using api.Interfaces;

namespace BE_Team7.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public readonly IEmailService _emailService;
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Guid> CreateOrderAsync(CreateOrderRequest request)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = new Order
                    {
                        Id = request.Id,
                        OrderDate = DateTime.UtcNow,
                        OrderStatus = "paying",
                        PromotionCode = "",
                        FinalAmount = request.FinalAmount,
                        TotalAmount = request.TotalAmount,
                        PromotionFee = 0,
                        PromotionId = null, // Để null nếu không có PromotionId
                        VoucherId = null, // Để null nếu không có VoucherId
                        ShippingFee = 0, // Phí vận chuyển (nếu có)
                        VoucherFee = 0, // Phí voucher (nếu có)
                        ShippingInfoId = null // Để ShippingInfoId là null
                    };

                    _context.Order.Add(order);
                    await _context.SaveChangesAsync();

                    if (request.Items != null)
                    {
                        foreach (var item in request.Items)
                        {
                            // Kiểm tra xem VariantId có tồn tại trong bảng ProductVariant không
                            var productVariant = await _context.ProductVariant
                                .FirstOrDefaultAsync(pv => pv.VariantId == item.VariantId);

                            if (productVariant == null)
                            {
                                throw new Exception($"ProductVariant with VariantId {item.VariantId} not found.");
                            }

                            var orderDetail = new OrderDetail
                            {
                                OrderId = order.OrderId, // Sử dụng order.OrderId thay vì order.Id
                                VariantId = item.VariantId,
                                Quantity = item.Quantity,
                                OrderDetailCreateAt = DateTime.UtcNow,
                            };
                            _context.OrderDetail.Add(orderDetail);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync(); // Sửa lại từ RollbackAsync thành CommitAsync

                    return order.OrderId;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Failed to create order: " + ex.Message);
                }
            }
        }

        public async Task<ApiResponse<List<OrderDto>>> GetOrdersByUserIdAsync(Guid userId)
        {
            try
            {
                // get danh sach don hang cua nguoi dung sap xep theo thoi gian giam dan
                var orders = await _context.Order
                    .Where(o => o.Id == userId)
                    .OrderByDescending(o => o.OrderDate)
                    .Include(o => o.ShippingInfo)
                    .Include(o => o.Promotion)
                    .Include(o => o.RefundInfo)
                    .Include(o => o.Voucher)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.ProductVariant)
                            .ThenInclude(pv => pv.Product)
                                .ThenInclude(p => p.Brand)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.ProductVariant)
                            .ThenInclude(pv => pv.Product)
                                .ThenInclude(p => p.Category)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.ProductVariant)
                            .ThenInclude(pv => pv.Product)
                                .ThenInclude(p => p.ProductAvatarImages)
                    .ToListAsync();
                if (orders == null || !orders.Any())
                {
                    return new ApiResponse<List<OrderDto>>()
                    {
                        Success = true,
                        Message = "User này không có Order nào",
                        Data = new List<OrderDto>()
                    };
                }
                // map dữ liệu từ Order sang OrderDto
                var orderDtos = (orders ?? Enumerable.Empty<Order>()).Select(o => new OrderDto 
                { 
                    OrderId = o.OrderId,
                    BrandName = o.OrderDetails.FirstOrDefault()?.ProductVariant?.Product?.Brand?.BrandName ?? "Unknown Brand",
                    BrandId = o.OrderDetails.FirstOrDefault()?.ProductVariant?.Product?.Brand?.BrandId ?? Guid.Empty,
                    CategoryName = o.OrderDetails.FirstOrDefault()?.ProductVariant?.Product?.Category?.CategoryName ?? "Unknown",
                    CategoryId = o.OrderDetails.FirstOrDefault()?.ProductVariant?.Product?.Category?.CategoryId ?? Guid.Empty,
                    OrderStatus = o.OrderStatus,
                    FinalAmount = o.FinalAmount,
                    OrderDate = o.OrderDate,
                    ShippingInfo = new ShippingInfoDto
                    {
                        Id = o.ShippingInfo?.Id ?? "unknow userId",
                        AddressType = o.ShippingInfo?.AddressType ?? "unknow AddressType",
                        LastName = o.ShippingInfo?.LastName ?? "unknow LastName",
                        FirstName = o.ShippingInfo?.FirstName ?? "unknow FirstName",
                        ShippingPhoneNumber = o.ShippingInfo?.ShippingPhoneNumber ?? "unknow ShippingPhoneNumber",
                        Province = o.ShippingInfo?.Province ?? "unknow Province",
                        District = o.ShippingInfo?.District ?? "unknow District",
                        Commune = o.ShippingInfo?.Commune ?? "unknow Commune",
                        AddressDetail = o.ShippingInfo?.AddressDetail ?? "unknow AddressDetail",
                        ShippingNote = o.ShippingInfo?.ShippingNote ?? "unknow ShippingNote"
                    },
                    orderRefundDto = new OrderRefundDto
                    {
                        RefundId = o.RefundInfo?.RefundId ?? Guid.Empty,
                        OrderRefundStatus = o.RefundInfo?.OrderRefundStatus ?? "unknow Status",
                        Reason = o.RefundInfo?.Reason ?? "unknow Reason",
                        AccountHolderName = o.RefundInfo?.AccountHolderName ?? "unknow Name",
                        BankAccountNumber = o.RefundInfo?.BankAccountNumber ?? "unknow SKT",
                        BankName = o.RefundInfo?.BankName ?? "Bank Name",
                        ProcessedDate = o.RefundInfo?.ProcessedDate ?? DateTime.MinValue,
                        RequestDate = o.RefundInfo?.RequestDate ?? DateTime.MinValue,
                    },
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                    {
                        ProductId = od.ProductVariant?.Product?.ProductId?? Guid.Empty,
                        ImageUrl = od.ProductVariant?.Product?.ProductAvatarImages?.FirstOrDefault()?.ImageUrl ?? "Unknown product avatar",
                        ProductName = od.ProductVariant?.Product?.ProductName?? "Unkonw product name",
                        Volume = od.ProductVariant?.Volume ?? 0,
                        SkinType = od.ProductVariant?.SkinType??"Unknow Skintype",
                        Price = od.ProductVariant?.Price ?? 0.0,
                        Quantity = od.Quantity
                    }).ToList()
                }).ToList();
                return new ApiResponse<List<OrderDto>>
                {
                    Success = true,
                    Message = "Orders retrieved successfully.",
                    Data = orderDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderDto>>
                {
                    Success = false,
                    Message = $"Failed to retrieve orders: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                // Lấy thông tin Order và các thông tin liên quan
                var order = await _context.Order
                    .Include(o => o.ShippingInfo)
                    .Include(o => o.Promotion)
                    .Include(o => o.Voucher)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.ProductVariant)
                            .ThenInclude(pv => pv.Product)
                                .ThenInclude(p => p.ProductAvatarImages)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return new ApiResponse<OrderResponseDto>
                    {
                        Success = false,
                        Message = "Order not found.",
                        Data = null
                    };
                }

                // Xử lý ShippingInfo nếu chưa có
                if (order.ShippingInfoId == null)
                {
                    var defaultShippingInfo = await _context.ShippingInfo
                        .FirstOrDefaultAsync(s => s.Id == order.Id.ToString() && s.DefaultAddress);

                    if (defaultShippingInfo != null)
                    {
                        // Cập nhật cả ShippingInfoId và navigation property
                        order.ShippingInfoId = Guid.Parse(defaultShippingInfo.Id);
                        order.ShippingInfo = defaultShippingInfo;

                        // Lưu thay đổi vào database nếu cần
                        await _context.SaveChangesAsync();
                    }
                }

                // Tính toán ShippingFee dựa trên Province
                decimal shippingFee = order.ShippingInfo?.Province == "Thành Phố Hồ Chí Minh" ? 0 : (order.ShippingInfo == null ? 0 : 35000);
                // Map dữ liệu từ Order sang OrderResponseDto
                var orderResponse = new OrderResponseDto
                {
                    OrderId = order.OrderId,
                    TotalAmount = order.TotalAmount,
                    FinalAmount = order.FinalAmount,
                    ShippingFee = shippingFee,
                    ShippingInfo = order.ShippingInfo,
                    OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                    {
                        ProductId = od.ProductVariant?.Product?.ProductId ?? Guid.Empty,
                        ImageUrl = od.ProductVariant?.Product?.ProductAvatarImages?.FirstOrDefault()?.ImageUrl ?? "Unknow Product Avatar",
                        ProductName = od.ProductVariant?.Product?.ProductName ?? "Unknow Product Name",
                        Volume = od.ProductVariant?.Volume ?? 0,
                        SkinType = od.ProductVariant?.SkinType ?? "Unknow Skin Type",
                        Price = od.ProductVariant?.Price ?? 0.0,
                        Quantity = od.Quantity
                    }).ToList()
                };

                return new ApiResponse<OrderResponseDto>
                {
                    Success = true,
                    Message = "Order retrieved successfully.",
                    Data = orderResponse
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderResponseDto>
                {
                    Success = false,
                    Message = $"Failed to retrieve order: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<bool>> UpdateShippingInfoIdAsync(Guid orderId, Guid? shippingInfoId)
        {
            try
            {
                // Tìm Order dựa trên OrderId
                var order = await _context.Order
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Order not found.",
                        Data = false
                    };
                }

                // Cập nhật ShippingInfoId
                order.ShippingInfoId = shippingInfoId;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.Order.Update(order);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "ShippingInfoId updated successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Failed to update ShippingInfoId: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<ApiResponse<bool>> UpdateVoucherAndPromotionAsync(Guid orderId, UpdateVoucherAndPromotionDto dto)
        {
            try
            {
                // Tìm Order dựa trên OrderId
                var order = await _context.Order
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Order not found.",
                        Data = false
                    };
                }

                // Cập nhật thông tin Voucher và Promotion
                order.PromotionId = dto.PromotionId;
                order.VoucherId = dto.VoucherId;
                order.VoucherFee = dto.VoucherFee;
                order.PromotionCode = dto.PromotionCode;
                order.PromotionFee = dto.PromotionFee;
                order.ShippingFee = dto.ShippingFee;
                order.FinalAmount = dto.FinalAmount;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.Order.Update(order);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Voucher and Promotion updated successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Failed to update Voucher and Promotion: {ex.Message}",
                    Data = false
                };
            }
        }
        public async Task<ApiResponse<List<OrderDto>>> GetOrdersByStatusAsync(Guid userId, string status)
        {
            try
            {
                // Lấy danh sách Order theo UserId và status
                var orders = await _context.Order
                    .Where(o => o.Id == userId && o.OrderStatus == status)
                    .OrderByDescending(o => o.OrderDate)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.ProductVariant)
                            .ThenInclude(pv => pv.Product)
                                .ThenInclude(p => p.Brand)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.ProductVariant)
                            .ThenInclude(pv => pv.Product)
                                .ThenInclude(p => p.Category)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.ProductVariant)
                            .ThenInclude(pv => pv.Product)
                                .ThenInclude(p => p.ProductAvatarImages)
                    .ToListAsync();

                if (orders == null || !orders.Any())
                {
                    return new ApiResponse<List<OrderDto>>
                    {
                        Success = true,
                        Message = "No orders found for this user and status.",
                        Data = new List<OrderDto>()
                    };
                }

                // Map dữ liệu từ Order sang OrderDto
                var orderDtos = orders.Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    BrandId = o.OrderDetails.FirstOrDefault()?.ProductVariant.Product.Brand.BrandId ?? Guid.Empty,
                    CategoryId = o.OrderDetails.FirstOrDefault()?.ProductVariant.Product.Category.CategoryId ?? Guid.Empty,
                    BrandName = o.OrderDetails.FirstOrDefault()?.ProductVariant.Product.Brand.BrandName ?? "Unknown Brand",
                    CategoryName = o.OrderDetails.FirstOrDefault()?.ProductVariant.Product.Category.CategoryName ?? "Unknown Category",
                    OrderStatus = o.OrderStatus,
                    FinalAmount = o.FinalAmount,
                    OrderDate = o.OrderDate,
                    ShippingInfo = new ShippingInfoDto
                {
                    Id = o.ShippingInfo?.Id?? "unknow userId",
                    AddressType = o.ShippingInfo?.AddressType ?? "unknow AddressType",
                    LastName = o.ShippingInfo?.LastName?? "unknow LastName",
                    FirstName = o.ShippingInfo?.FirstName?? "unknow FirstName",
                    ShippingPhoneNumber = o.ShippingInfo?.ShippingPhoneNumber?? "unknow ShippingPhoneNumber",
                    Province = o.ShippingInfo?.Province?? "unknow Province",
                    District = o.ShippingInfo?.District?? "unknow District",
                    Commune = o.ShippingInfo?.Commune?? "unknow Commune",
                    AddressDetail = o.ShippingInfo?.AddressDetail?? "unknow AddressDetail",
                    ShippingNote = o.ShippingInfo?.ShippingNote?? "unknow ShippingNote"
                },
                orderRefundDto = new OrderRefundDto
                {
                    RefundId = o.RefundInfo?.RefundId ?? Guid.Empty,
                    OrderRefundStatus = o.RefundInfo?.OrderRefundStatus ?? "unknow Status",
                    Reason = o.RefundInfo?.Reason ?? "unknow Reason",
                    AccountHolderName = o.RefundInfo?.AccountHolderName ?? "unknow Name",
                    BankAccountNumber = o.RefundInfo?.BankAccountNumber ?? "unknow SKT",
                    BankName = o.RefundInfo?.BankName ?? "Bank Name",
                    ProcessedDate = o.RefundInfo?.ProcessedDate ?? DateTime.MinValue,
                    RequestDate = o.RefundInfo?.RequestDate ?? DateTime.MinValue,
                },
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                    {
                        ProductId = od.ProductVariant?.Product?.ProductId ?? Guid.Empty,
                        ImageUrl = od.ProductVariant?.Product?.ProductAvatarImages?.FirstOrDefault()?.ImageUrl ?? "Unknow Product Avatar",
                        ProductName = od.ProductVariant?.Product?.ProductName ?? "Unknow Product Name",
                        Volume = od.ProductVariant?.Volume ?? 0,
                        SkinType = od.ProductVariant?.SkinType ?? "Unknow Skin Type",
                        Price = od.ProductVariant?.Price ?? 0.0,
                        Quantity = od.Quantity
                    }).ToList()
                }).ToList();

                return new ApiResponse<List<OrderDto>>
                {
                    Success = true,
                    Message = "Orders retrieved successfully.",
                    Data = orderDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderDto>>
                {
                    Success = false,
                    Message = $"Failed to retrieve orders: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<List<ManageOrderDto>> GetOrdersByStatusManageAsync(string status)
        {
            var orders = await _context.Order
                .Where(o => o.OrderStatus.ToLower() == status.ToLower()) // Lọc theo trạng thái nhập vào
                .OrderBy(o => o.OrderDate)
                .Include(o => o.Payments)
                .Include(o => o.RefundInfo)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                        .ThenInclude(pv => pv.Product) // Lấy thông tin Product từ ProductVariant
                .Include(o => o.ShippingInfo)
                .ToListAsync();
            var result = orders.Select(o => new ManageOrderDto
            {
                PaymentId = (o.Payments != null && o.Payments.Any()) ? o.Payments.First().PaymentId : Guid.Empty,
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                ShippingFee = o.ShippingFee,
                VoucherFee = o.VoucherFee,
                PromotionFee = o.PromotionFee,
                TotalAmount = o.TotalAmount,
                FinalAmount = o.FinalAmount,
                ShippingInfo = new ShippingInfoDto
                {
                    Id = o.ShippingInfo?.Id?? "unknow userId",
                    AddressType = o.ShippingInfo?.AddressType ?? "unknow AddressType",
                    LastName = o.ShippingInfo?.LastName?? "unknow LastName",
                    FirstName = o.ShippingInfo?.FirstName?? "unknow FirstName",
                    ShippingPhoneNumber = o.ShippingInfo?.ShippingPhoneNumber?? "unknow ShippingPhoneNumber",
                    Province = o.ShippingInfo?.Province?? "unknow Province",
                    District = o.ShippingInfo?.District?? "unknow District",
                    Commune = o.ShippingInfo?.Commune?? "unknow Commune",
                    AddressDetail = o.ShippingInfo?.AddressDetail?? "unknow AddressDetail",
                    ShippingNote = o.ShippingInfo?.ShippingNote?? "unknow ShippingNote"
                },
                orderRefundDto = new OrderRefundDto
                {
                    RefundId = o.RefundInfo?.RefundId??Guid.Empty,
                    OrderRefundStatus = o.RefundInfo?.OrderRefundStatus?? "unknow Status",
                    Reason = o.RefundInfo?.Reason?? "unknow Reason",
                    AccountHolderName = o.RefundInfo?.AccountHolderName?? "unknow Name",
                    BankAccountNumber = o.RefundInfo?.BankAccountNumber?? "unknow SKT",
                    BankName = o.RefundInfo?.BankName?? "Bank Name",
                    ProcessedDate = o.RefundInfo?.ProcessedDate?? DateTime.MinValue,
                    RequestDate =  o.RefundInfo?.RequestDate?? DateTime.MinValue,                   
                },
                ManageOrderDetail = o.OrderDetails.Select(od => new ManageOrderDetailDto
                {
                    VariantId = od.VariantId,
                    ProductId = od.ProductVariant?.Product?.ProductId ?? Guid.Empty,
                    ImageUrl = od.ProductVariant?.Product?.ProductAvatarImages?.FirstOrDefault()?.ImageUrl ?? "Unknow Product Avatar",
                    ProductName = od.ProductVariant?.Product?.ProductName ?? "Unknow Product Name",
                    Volume = od.ProductVariant?.Volume ?? 0,
                    SkinType = od.ProductVariant?.SkinType ?? "Unknow Skin Type",
                    Price = od.ProductVariant?.Price ?? 0,
                    Quantity = od.Quantity
                }).ToList()
            }).ToList();
            return result;
        }
        public async Task<ApiResponse<string>> UpdateOrderStatusAsync(Guid orderId, string status)
        {
            try
            {
                // Tìm đơn hàng theo orderId
                var order = await _context.Order.FindAsync(orderId);
                if (order == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Đơn hàng không tồn tại.",
                        Data = null
                    };
                }

                // Cập nhật trạng thái
                order.OrderStatus = status;
                _context.Order.Update(order);

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Cập nhật trạng thái đơn hàng thành công.",
                    Data = status
                };
            }
            catch (Exception ex)
            {
                // Trả về phản hồi lỗi nếu có exception
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Đã xảy ra lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<OrderRefund>> CreateRefundAsync(Guid orderId, string reason)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Truy vấn đơn hàng trực tiếp kèm thông tin hoàn tiền
                var order = await _context.Order
                    .Include(o => o.RefundInfo)
                    .Include(o => o.User)// Quan trọng: Load cả thông tin RefundInfo
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return new ApiResponse<OrderRefund>
                    {
                        Success = false,
                        Message = "Đơn hàng không tồn tại",
                        Data = null
                    };
                }

                if (order.RefundInfo != null)
                {
                    return new ApiResponse<OrderRefund>
                    {
                        Success = false,
                        Message = "Đơn hàng đã có yêu cầu hoàn tiền",
                        Data = null
                    };
                }

                var refund = new OrderRefund
                {
                    OrderId = orderId,
                    Reason = reason,
                    OrderRefundStatus = "Pending",
                    RequestDate = DateTime.UtcNow
                };

                order.OrderStatus = "Returning";
                _context.orderRefunds.Add(refund);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // Xác nhận transaction
                var userModels = await _context.User
                    .FirstOrDefaultAsync(o => o.Id.ToString() == order.Id.ToString());
                Console.WriteLine("sdkjfg"+ userModels.Email);
                if (userModels == null)
                {
                    return new ApiResponse<OrderRefund>
                    {
                        Success = false, 
                        Message = "Không tìm thấy thông tin người dùng cho đơn hàng này",
                        Data = null
                    };
                }
                if (string.IsNullOrEmpty(userModels.Email))
                {
                    return new ApiResponse<OrderRefund>
                    {
                        Success = false,
                        Message = "Không tìm thấy email của người dùng",
                        Data = null
                    };
                }
                try
                {
                    var emailSubject = "Yêu cầu hoàn tiền đã được tạo";
                    var emailBody = $@"
                <h3>Kính gửi {userModels.Name},</h3>
                <p>Yêu cầu hoàn tiền cho đơn hàng #{order.OrderId} đã được tạo thành công.</p>
                <p><strong>Lý do:</strong> {reason}</p>
                <p><strong>Mã yêu cầu hoàn tiền:</strong> {refund.RefundId}</p>
                <p>Vui lòng cung cấp thông tin tài khoản ngân hàng để chúng tôi xử lý hoàn tiền.</p>
                <p>Trân trọng,</p>
                <p>Đội ngũ hỗ trợ</p>";

                    await _emailService.SendEmailAsync(userModels.Email, emailSubject, emailBody);
                }
                catch (Exception emailEx)
                {
                    await transaction.RollbackAsync();
                    return new ApiResponse<OrderRefund>
                    {
                        Success = false,
                        Message = "Lỗi hệ thống khi gửi email hoàn tiền",
                        Data = null
                    };
                }
                return new ApiResponse<OrderRefund>
                {
                    Success = true,
                    Message = "Tạo yêu cầu hoàn tiền thành công",
                    Data = refund
                };
            }

            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<OrderRefund>
                {
                    Success = false,
                    Message = "Lỗi hệ thống khi tạo hoàn tiền",
                    Data = null
                };
            }
        }
        public async Task<OrderRefund> UpdateCustomerRefundInfo(CustomerRefundUpdateDto updateDto)
        {
            // Lấy thông tin hoàn tiền thông qua OrderId
            var refund = await _context.orderRefunds
                .FirstOrDefaultAsync(r => r.OrderId == updateDto.OrderId);

            if (refund == null)
                throw new ArgumentException("Không tìm thấy yêu cầu hoàn tiền cho đơn hàng này");

            if (refund.OrderRefundStatus != "Pending")
                throw new InvalidOperationException("Chỉ có thể cập nhật thông tin khi trạng thái là Pending");

            // Cập nhật thông tin khách hàng
            refund.BankAccountNumber = updateDto.BankAccountNumber;
            refund.BankName = updateDto.BankName;
            refund.AccountHolderName = updateDto.AccountHolderName;
            // Chuyển trạng thái sang Checking
            refund.OrderRefundStatus = "Checking";
            await _context.SaveChangesAsync();
            return refund;
        }
        public async Task<List<ManageOrderDto>> GetOrdersByRefundStatusAsync(string refundStatus)
        {
            var orders = await _context.Order
                .Where(o => o.RefundInfo != null &&
                           o.RefundInfo.OrderRefundStatus.ToLower() == refundStatus.ToLower())
                .OrderBy(o => o.OrderDate)
                .Include(o => o.Payments)
                .Include(o => o.RefundInfo)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                .Include(o => o.ShippingInfo)
                .ToListAsync();

            var result = orders.Select(o => new ManageOrderDto
            {
                PaymentId = o.Payments.FirstOrDefault()?.PaymentId ?? Guid.Empty,
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                ShippingFee = o.ShippingFee,
                VoucherFee = o.VoucherFee,
                PromotionFee = o.PromotionFee,
                TotalAmount = o.TotalAmount,
                FinalAmount = o.FinalAmount,
                ShippingInfo = new ShippingInfoDto
                {
                    Id = o.ShippingInfo?.Id ?? "unknown userId",
                    AddressType = o.ShippingInfo?.AddressType ?? "unknown AddressType",
                    LastName = o.ShippingInfo?.LastName ?? "unknown LastName",
                    FirstName = o.ShippingInfo?.FirstName ?? "unknown FirstName",
                    ShippingPhoneNumber = o.ShippingInfo?.ShippingPhoneNumber ?? "unknown ShippingPhoneNumber",
                    Province = o.ShippingInfo?.Province ?? "unknown Province",
                    District = o.ShippingInfo?.District ?? "unknown District",
                    Commune = o.ShippingInfo?.Commune ?? "unknown Commune",
                    AddressDetail = o.ShippingInfo?.AddressDetail ?? "unknown AddressDetail",
                    ShippingNote = o.ShippingInfo?.ShippingNote ?? "unknown ShippingNote"
                },
                orderRefundDto = new OrderRefundDto
                {
                    RefundId = o.RefundInfo?.RefundId ?? Guid.Empty,
                    OrderRefundStatus = o.RefundInfo?.OrderRefundStatus ?? "unknown Status",
                    Reason = o.RefundInfo?.Reason ?? "unknown Reason",
                    AccountHolderName = o.RefundInfo?.AccountHolderName ?? "unknown Name",
                    BankAccountNumber = o.RefundInfo?.BankAccountNumber ?? "unknown SKT",
                    BankName = o.RefundInfo?.BankName ?? "unknown Bank Name",
                    ProcessedDate = o.RefundInfo?.ProcessedDate ?? DateTime.MinValue,
                    RequestDate = o.RefundInfo?.RequestDate ?? DateTime.MinValue,
                },
                ManageOrderDetail = o.OrderDetails.Select(od => new ManageOrderDetailDto
                {
                    VariantId = od.VariantId,
                    ProductId = od.ProductVariant?.Product?.ProductId ?? Guid.Empty,
                    ImageUrl = od.ProductVariant?.Product?.ProductAvatarImages?.FirstOrDefault()?.ImageUrl ?? "unknown Product Avatar",
                    ProductName = od.ProductVariant?.Product?.ProductName ?? "unknown Product Name",
                    Volume = od.ProductVariant?.Volume ?? 0,
                    SkinType = od.ProductVariant?.SkinType ?? "unknown Skin Type",
                    Price = od.ProductVariant?.Price ?? 0,
                    Quantity = od.Quantity
                }).ToList()
            }).ToList();

            return result;
        }
    }
}
