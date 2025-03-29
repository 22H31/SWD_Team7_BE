using BE_Team7.Dtos.ShippingInfo;

namespace BE_Team7.Dtos.Order
{
    public class ManageOrderDto
    {
        public Guid? PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal VoucherFee { get; set; }
        public decimal PromotionFee { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public required ShippingInfoDto ShippingInfo { get; set; }
        public required List<ManageOrderDetailDto> ManageOrderDetail { get; set; }
        public required OrderRefundDto orderRefundDto { get; set; }
    }
}
