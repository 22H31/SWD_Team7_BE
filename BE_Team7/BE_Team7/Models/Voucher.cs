using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid VoucherId { get; set; }

        [Required]
        public string VoucherName { get; set; } = string.Empty;

        [Required]
        public string VoucherDescription { get; set; } = string.Empty;

        public decimal VoucherRate { get; set; }
        public int VoucherQuantity { get; set; }
        [Required]
        public DateTime VoucherStartDate { get; set; }
        [Required]
        public DateTime VoucherEndDate { get; set; }

        // Cho phép null cho CategoryId
        public Guid? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        // Cho phép null cho BrandId
        public Guid? BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand? Brand { get; set; }
    }
}
