namespace BE_Team7.Dtos.FeedBack
{
    public class FeedbackDto
    {
        public Guid FeedbackId { get; set; }
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public required string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool HasPurchased { get; set; }
    }
}
