namespace BE_Team7.Dtos.Refund
{
    public class RefundResponseDto
    {
        public Guid RefundId { get; set; }
        public required string OrderRefundStatus { get; set; } = null!;
    }
}
