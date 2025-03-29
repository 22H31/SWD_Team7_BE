namespace BE_Team7.Dtos.Order
{
    public class CreateOrderRequest
    {
        public Guid Id { get; set; } 
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public List<OrderItemDto>? Items { get; set; } 
    }
}
