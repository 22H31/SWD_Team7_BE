using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Blog
    {
        [Key]
        public Guid BlogId { get; set; } // Primary Key

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        [Required]
        public string Content1 { get; set; } = string.Empty;
        public string Content2 { get; set; } = string.Empty;
        public DateTime BlogCreatedAt { get; set; } = DateTime.UtcNow;
        // Danh sách hình ảnh sản phẩm
        public virtual ICollection<BlogAvartarImage> BlogAvartarImage { get; set; } = new List<BlogAvartarImage>();
        public virtual ICollection<BlogImage> BlogImage { get; set; } = new List<BlogImage>();

        [ForeignKey("Account")]
        public Guid Id { get; set; } // Foreign Key

        public virtual User User { get; set; } = null!; // Load Account để lấy AuthorName
    
    }
}
