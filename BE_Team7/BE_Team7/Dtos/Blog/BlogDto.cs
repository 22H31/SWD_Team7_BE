namespace BE_Team7.Dtos.Blog
{
    public class BlogDto
    {
        public Guid BlogId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid Id { get; set; }
        public string AuthorName { get; set; } = string.Empty; // Lấy tên tác giả từ Account
    }
}
