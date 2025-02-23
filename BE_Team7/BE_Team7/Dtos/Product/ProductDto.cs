using BE_Team7.Models;

namespace BE_Team7.Dtos.Product
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string SkinType { get; set; }
        public string CategoryName { get; set; }
    }
}
