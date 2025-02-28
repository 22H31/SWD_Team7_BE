using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public required string ProductName { get; set; }
        public required string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public required string SkinType { get; set; }
        public required string ProductImg { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Category Category { get; set; } = null!;
        public virtual Brand Brand { get; set; } = null!;

    }
}
