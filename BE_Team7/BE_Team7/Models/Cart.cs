using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CartId { get; set; } 
        public Guid Id { get; set; }
        public virtual User User { get; set; }=null!;

        public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Danh sách sản phẩm trong giỏ


    }
}
