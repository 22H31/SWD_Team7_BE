namespace BE_Team7.Dtos.Promotion
{
    public class CreatePromotionDto
    {
        public required string PromotionName { get; set; }
        public required string PromotionCode { get; set; }
        public required string PromotionDescription { get; set; }
        public decimal DiscountRate { get; set; }
        public DateTime PromotionStartDate { get; set; }
        public DateTime PromotionEndDate { get; set; }
    }
}
