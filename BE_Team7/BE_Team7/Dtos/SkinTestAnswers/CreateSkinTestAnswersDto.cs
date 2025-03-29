using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Dtos.SkinTestAnswers
{
    public class CreateSkinTestAnswersDto
    {
        [Required]
        public Guid QuestionId { get; set; }
        public required string AnswerDetail { get; set; }
        public int SkinNormalScore { get; set; }
        public int SkinDryScore { get; set; }
        public int SkinOilyScore { get; set; }
        public int SkinCombinationScore { get; set; }
        public int SkinSensitiveScore { get; set; }
    }
}
