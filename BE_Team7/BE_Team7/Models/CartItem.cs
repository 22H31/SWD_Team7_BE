using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int CartItemId { get; set; }
        public virtual Cart Cart { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
