namespace BE_Team7.Dtos.FeedBack
{
    public class FeedbackDto
    {
        public Guid FeedbackId { get; set; }
        public string UserAvatarUrl { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string CreatedAt { get; set; }
    }
}