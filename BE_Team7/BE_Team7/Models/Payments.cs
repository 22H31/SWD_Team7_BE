using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Payments
    {
        [Key]  // Định nghĩa khóa chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? PaymentId { get; set; }
        public Guid? OrderId { get; set; } // Thay đổi từ string sang Guid
        public string OrderDescription { get; set; }
        public string TransactionId { get; set; }
        public string PaymentMethod { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }

        // Thêm ràng buộc khóa ngoại
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;
    }
}