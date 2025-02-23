namespace BE_Team7.Dtos.Product
{
    public class UpdateProductRequestDto
    {
        public Guid CategoryId { get; set; }
        public required string ProductName { get; set; }
        public required string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public required string SkinType { get; set; }
        public required string Img { get; set; }
    }
}
