using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class ProductVariant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid VariantId { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
        public int Volume { get; set; }
        public required string SkinType { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public required string MainIngredients { get; set; }
        public required string FullIngredients { get; set; }
    }
}