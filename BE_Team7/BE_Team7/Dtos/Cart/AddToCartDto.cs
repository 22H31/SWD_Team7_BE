namespace BE_Team7.Dtos.Cart
{
    public class AddToCartDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
