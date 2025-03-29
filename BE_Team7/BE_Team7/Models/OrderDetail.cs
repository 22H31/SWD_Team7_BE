using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderDetailId { get; set; }
        [ForeignKey("Order")]
        public Guid? OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
        public int Quantity { get; set; }

        [ForeignKey("ProductVariant")]
        public required Guid VariantId { get; set; }
        public virtual ProductVariant ProductVariant { get; set; } = null!;
        public DateTime OrderDetailCreateAt { get; set; }
    }
}
