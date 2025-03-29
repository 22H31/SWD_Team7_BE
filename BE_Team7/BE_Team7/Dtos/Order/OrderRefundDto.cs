using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Dtos.Order
{
    public class OrderRefundDto
    {
        public Guid RefundId { get; set; }
        public required string Reason { get; set; } // Lý do từ nhân viên
        public DateTime RequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; } // Ngày hoàn tiền xong
        public required string? OrderRefundStatus { get; set; } // Pending, Completed, Failed
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountHolderName { get; set; }
    }
}
