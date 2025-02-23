using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FeedbackId { get; set; }
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public required string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;

    }
}
