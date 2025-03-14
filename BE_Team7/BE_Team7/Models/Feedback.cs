using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FeedbackId { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } = null!;
        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual User User { get; set; } = null!; 
        public int Rating { get; set; }
        public required string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}