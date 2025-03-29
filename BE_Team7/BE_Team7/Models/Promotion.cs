// Models/Promotion.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PromotionId { get; set; }

        [Required]
        public required string PromotionName { get; set; }

        [Required]
        public required string PromotionCode { get; set; }

        [Required]
        public required string PromotionDescription { get; set; }

        [Required]
        public decimal DiscountRate { get; set; } 

        [Required]
        public DateTime PromotionStartDate { get; set; }

        [Required]
        public DateTime PromotionEndDate { get; set; }
    }
}