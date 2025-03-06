namespace BE_Team7.Dtos.FeedBack
{
    public class UpdateFeedbackRequestDto
    {
        public int Rating { get; set; }
        public required string Comment { get; set; }
    }
}
