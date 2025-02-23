using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_Team7.Models
{
    public class SkinTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QuestionId { get; set; }
        public required string QuestionDetail { get; set; }
        public required string OptionA { get; set; }
        public required string OptionB { get; set; }
        public required string OptionC { get; set; }
        public required string OptionD { get; set; }
    }
}
