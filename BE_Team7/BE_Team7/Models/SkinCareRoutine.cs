using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class SkinCareRoutine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoutineId { get; set; }
        public Guid Id { get; set; }
        public required string Purpose { get; set; }
        public required string RoutineDescription { get; set; }
        public required string RoutineStatus { get; set; }
        public required string SkinCondition { get; set; }
        public required string SkinImg { get; set; }
        public required string Other { get; set; }
        public virtual User User { get; set; } = null!;

    }
}
