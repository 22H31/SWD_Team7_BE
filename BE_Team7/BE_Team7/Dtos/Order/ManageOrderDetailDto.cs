namespace BE_Team7.Dtos.Order
{
    public class ManageOrderDetailDto
    {
        public Guid VariantId { get; set; }
        public Guid ProductId { get; set; }
        public required string ImageUrl { get; set; }
        public required string ProductName { get; set; }
        public int Volume { get; set; }
        public required string SkinType { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
