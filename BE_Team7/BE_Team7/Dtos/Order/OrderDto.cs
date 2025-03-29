using BE_Team7.Dtos.ShippingInfo;

namespace BE_Team7.Dtos.Order
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public required string BrandName { get; set; }
        public required string CategoryName { get; set; }
        public required string OrderStatus { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public required List<OrderDetailDto> OrderDetails { get; set; }
        public required ShippingInfoDto ShippingInfo { get; set; }
        public required OrderRefundDto orderRefundDto { get; set; }
    }
}
