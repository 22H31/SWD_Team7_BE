using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Models
{
    public class SkinTestAnswers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AnswerId { get; set; }

        [Required]
        [ForeignKey("SkinTestQuestion")]
        public Guid QuestionId { get; set; }

        [Required]
        public required string AnswerDetail { get; set; } = string.Empty;
        public int SkinNormalScore { get; set; }
        public int SkinDryScore { get; set; }
        public int SkinOilyScore { get; set; }
        public int SkinCombinationScore { get; set; }
        public int SkinSensitiveScore { get; set; }
        public virtual SkinTestQuestion SkinTestQuestion { get; set; } = null!;
    }
}
