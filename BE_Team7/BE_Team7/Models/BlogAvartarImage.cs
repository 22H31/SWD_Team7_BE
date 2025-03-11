using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class BlogAvartarImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Tự động tạo giá trị
        public Guid BlogAvartarImageId { get; set; }
        public string? ImageId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime BlogAvartarImageCreatedAt { get; set; }
        public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; } = null!;
    }
}
