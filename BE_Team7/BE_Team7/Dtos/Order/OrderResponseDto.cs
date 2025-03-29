using BE_Team7.Models;

namespace BE_Team7.Dtos.Order
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public required BE_Team7.Models.ShippingInfo ShippingInfo { get; set; }
        public required List<OrderDetailDto> OrderDetails { get; set; }
    }
}
