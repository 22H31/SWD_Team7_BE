namespace BE_Team7.Dtos.ProductVariant
{
    public class CreateProductVariantRequestDto
    {
        public Guid ProductId { get; set; }
        public int Volume { get; set; }
        public required string SkinType { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public required string MainIngredients { get; set; }
        public required string FullIngredients { get; set; }
    }
}
