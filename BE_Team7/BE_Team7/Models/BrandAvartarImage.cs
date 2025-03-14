using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class BrandAvartarImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Tự động tạo giá trị
        public Guid BrandAvartarImageId { get; set; }
        public string? ImageId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime BrandAvartarImageCreatedAt { get; set; }
        public Guid BrandId { get; set; }
        public virtual Brand Brand { get; set; } = null!;
    }
}
