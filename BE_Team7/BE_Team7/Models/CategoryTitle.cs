using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class CategoryTitle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryTitleId { get; set; }
        public required string CategoryTitleName { get; set; }
        public required string CategoryTitleIcon { get; set; }
    }
}
