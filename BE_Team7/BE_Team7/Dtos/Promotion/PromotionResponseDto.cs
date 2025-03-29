namespace BE_Team7.Dtos.Promotion
{
    public class PromotionResponseDto
    {
        public Guid PromotionId { get; set; }
        public decimal DiscountRate { get; set; }
        public required string Message { get; set; }
    }
}
