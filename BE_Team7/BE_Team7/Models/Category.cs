using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required Guid CategoryId { get; set; }
        public required Guid CategoryTitleId { get; set; }
        public required string CategoryName { get; set; }
        public required string Description { get; set; }
        public virtual CategoryTitle CategoryTitle { get; set; } = null!;
    }
}
