using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CartItemId { get; set; }

        [ForeignKey("User")]
        public required string Id { get; set; }  
        public virtual User User { get; set; } = null!;

        [ForeignKey("ProductVariant")]
        public required Guid VariantId { get; set; }
        public virtual ProductVariant ProductVariant { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime CartItemCreateAt { get; set; }
    }
}
