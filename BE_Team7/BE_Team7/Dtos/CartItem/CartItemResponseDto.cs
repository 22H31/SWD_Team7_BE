namespace BE_Team7.Dtos.CartItem
{
    public class CartItemResponseDto
    {
        public Guid VariantId { get; set; }
        public Guid CartItemId { get; set; }
        public Guid ProductId { get; set; }
        public required string ProductAvatarImage { get; set; }
        public required string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public required string SkinType { get; set; } = null!;
    }
}
