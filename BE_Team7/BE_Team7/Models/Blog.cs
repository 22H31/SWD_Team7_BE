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

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Account")]
        public Guid Id { get; set; } // Foreign Key

        public virtual User User { get; set; } = null!; // Load Account để lấy AuthorName
    }
}
