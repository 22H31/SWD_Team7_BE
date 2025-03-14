namespace BE_Team7.Dtos.Blog
{
    public class BlogDetailDto
    {
        public Guid BlogId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string Content1 { get; set; } = string.Empty;
        public string Content2 { get; set; } = string.Empty;
        public Dictionary<string, string> BlogImageUrl { get; set; }
        public string BlogAvartarImageUrl { get; set; }
        public DateTime BlogCreatedAt { get; set; }
    }
}