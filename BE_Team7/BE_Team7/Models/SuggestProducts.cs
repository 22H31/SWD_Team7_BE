using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class SuggestProducts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SuggestProductsId { get; set; }
        public Guid ProductId { get; set; }
        public Guid RoutineId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual SkinCareRoutine SkinCareRoutine { get; set; }=null!;
    }
}
