using BE_Team7.Dtos.SkinTestAnswers;

namespace BE_Team7.Dtos.SkinTestQuestion
{
    public class SkinTestQuestionDto
    {
        public Guid QuestionId { get; set; }
        public required string QuestionDetail { get; set; }
        public List<SkinTestAnswerDto> Answers { get; set; } = new List<SkinTestAnswerDto>();

    }
}
