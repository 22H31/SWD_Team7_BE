namespace BE_Team7.Dtos.FeedBack
{
    public class CreateFeebackRequestDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public required string Comment { get; set; }
    }
}
