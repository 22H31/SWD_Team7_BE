namespace BE_Team7.Dtos.CartItem
{
    public class CreateCartItemDto
    {
        public string UserId { get; set; }
        public string VariantId { get; set; }
        public int Quantity { get; set; }
    }
}
