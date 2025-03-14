using BE_Team7.Dtos.ProductVariant;

namespace BE_Team7.Dtos.Product
{
    public class ProductResponseDto
    {
        public int SoldQuantity { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string AvartarImageUrl { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public double AverageRating { get; set; }
        public int TotalFeedback { get; set; }
        public List<ProductVariantDto> Variants { get; set; }
    }
}