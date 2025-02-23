using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PromotionId { get; set; }
        public required string PromotionName { get; set; }
        public required string PromotionCode { get; set; }
        public required string PromotionDescription { get; set; }
        public required string PromotionType { get; set; }
        public required string DiscountRate { get; set; }
        public DateTime PromotionStartDate { get; set; }
        public DateTime PromotionEndDate { get; set; }
    }
}
