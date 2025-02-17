namespace BE_Team7.Models
{
    public class ProductIf
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string SkinType { get; set; }
        public string CreatedAt { get; set; }

    }
    public class Product : ProductIf
    {
        public Guid ProductId { get; set; }
    }
}
