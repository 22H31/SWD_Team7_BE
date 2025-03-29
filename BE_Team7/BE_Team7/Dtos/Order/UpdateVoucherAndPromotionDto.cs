namespace BE_Team7.Dtos.Order
{
    public class UpdateVoucherAndPromotionDto
    {
        public required Guid? PromotionId { get; set; }
        public required Guid? VoucherId { get; set; }
        public decimal VoucherFee { get; set; }
        public required string? PromotionCode { get; set; }
        public decimal PromotionFee { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal ShippingFee { get; set; }
    }
}
