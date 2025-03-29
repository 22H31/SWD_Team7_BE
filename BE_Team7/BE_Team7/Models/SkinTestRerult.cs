using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class SkinTestRerult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Tự động tạo giá trị
        public Guid RerultId { get; set; }
        public double TotalSkinNormalScore { get; set; }
        public double TotalSkinDryScore { get; set; }
        public double TotalSkinOilyScore { get; set; }
        public double TotalSkinCombinationScore { get; set; }
        public double TotalSkinSensitiveScore { get; set; }
        public string? SkinType { get; set; }
        public DateTime RerultCreateAt { get; set; }
        [ForeignKey("User")]
        public string Id { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
