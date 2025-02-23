using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public required string OrderStatus { get; set; }
        public required string PromotionCode { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Promotion Promotion { get; set; }=null!;
    }
}
