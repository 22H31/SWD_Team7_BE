using BE_Team7.Dtos.FeedBack;
using BE_Team7.Dtos.ProductVariant;
using BE_Team7.Models;

namespace BE_Team7.Dtos.Product
{
    public class ProductDetailDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public double AverageRating { get; set; }
        public int TotalFeedback { get; set; }
        public Dictionary<string, string> ImageUrl { get; set; }
        public string AvatarImageUrl { get; set; }
        public List<ProductVariantDto> Variants { get; set; }
        public DescriptionDto Describe { get; set; }
        public SpecificationDto Specifications { get; set; }
        public UseManualDto UseManual { get; set; }
        public List<FeedbackDto> Feedbacks { get; set; }
    }
}