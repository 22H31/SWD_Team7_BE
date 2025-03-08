namespace BE_Team7.Dtos.Product
{
    public class UpdateProductRequestDto
    {
        public required string ProductName { get; set; }
        public required string ProductAvatar { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }
        public Dictionary<string, string> ImageUrl { get; set; } = new();
        public List<ProductVariantDto> Variants { get; set; } = new();
        public DescriptionDto Describe { get; set; } = new();
        public SpecificationDto Specification { get; set; } = new();
        public UseManualDto UseManual { get; set; } = new();
    }
}
