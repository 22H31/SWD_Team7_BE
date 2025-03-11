using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class BlogImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Tự động tạo giá trị
        public Guid BlogImageId { get; set; }
        public string? ImageId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime BlogImageCreatedAt { get; set; }
        public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; } = null!;
    }
}
