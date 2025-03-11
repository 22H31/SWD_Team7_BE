namespace BE_Team7.Dtos.Product
{
    public class UpdateProductRequestDto
    {
        public required string ProductName { get; set; }
        public required Guid BrandId { get; set; }
        public required Guid CategoryId { get; set; }
        public required DescriptionDto Describe { get; set; }
        public required SpecificationDto Specifications { get; set; }
        public required UseManualDto UseManual { get; set; }
    }
}
