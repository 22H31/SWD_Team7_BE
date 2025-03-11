using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Tự động tạo giá trị
        public Guid ProductImageId { get; set; }
        public string? ImageId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime ProductImageCreatedAt { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

    }
}