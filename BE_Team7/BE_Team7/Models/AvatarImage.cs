using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class AvatarImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Tự động tạo giá trị
        public Guid UserImageId { get; set; }
        public string? ImageId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime AvatarImageCreatedAt { get; set; }
        public string Id { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
