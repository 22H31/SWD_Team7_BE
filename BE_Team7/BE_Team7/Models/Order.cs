using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }
        public Guid? ShippingInfoId { get; set; }
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public required string OrderStatus { get; set; }
        public required string PromotionCode { get; set; }
        public Guid? VoucherId { get; set; }
        public Guid? PromotionId { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal VoucherFee { get; set; }
        public decimal PromotionFee { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public virtual User? User { get; set; } = null!; 
        public virtual Promotion? Promotion { get; set; }=null!;
        public virtual Voucher? Voucher { get; set; }=null!;
        public virtual ShippingInfo? ShippingInfo { get; set; }=null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual ICollection<Payments> Payments { get; set; } = new List<Payments>();
        public virtual OrderRefund? RefundInfo { get; set; } = null!;
    }
}
