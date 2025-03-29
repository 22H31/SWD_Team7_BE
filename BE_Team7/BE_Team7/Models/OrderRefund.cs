using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class OrderRefund
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RefundId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public required string Reason { get; set; } // Lý do từ nhân viên

        [Required]
        public DateTime RequestDate { get; set; }

        public DateTime? ProcessedDate { get; set; } // Ngày hoàn tiền xong

        [Required]
        public required string OrderRefundStatus { get; set; } // Pending, Completed, Failed

        // Thông tin STK khách hàng (do họ nhập)
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountHolderName { get; set; }

        // Liên kết với Order
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;
    }
}
