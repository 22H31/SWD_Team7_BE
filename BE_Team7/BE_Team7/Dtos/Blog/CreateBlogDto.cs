namespace BE_Team7.Dtos.Blog
{
    public class CreateBlogDto
    {
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string Content1 { get; set; } = string.Empty;
        public string Content2 { get; set; } = string.Empty;
        public required Guid Id { get; set; } // Người viết bài
    }
}