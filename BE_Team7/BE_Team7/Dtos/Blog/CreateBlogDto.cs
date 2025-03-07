namespace BE_Team7.Dtos.Blog
{
    public class CreateBlogDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid Id { get; set; } // Người viết bài
    }
}
