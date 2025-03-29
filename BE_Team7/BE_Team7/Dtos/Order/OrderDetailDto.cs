namespace BE_Team7.Dtos.Order
{
    public class OrderDetailDto
    {
        public Guid ProductId { get; set; }
        public required string ImageUrl { get; set; }
        public required string ProductName { get; set; }
        public  int Volume { get; set; }
        public required string SkinType { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
