namespace BE_Team7.Dtos.Blog
{
    public class BlogDto
    {
        public Guid BlogId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string Content1 { get; set; } = string.Empty;
        public string AvartarBlogUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
