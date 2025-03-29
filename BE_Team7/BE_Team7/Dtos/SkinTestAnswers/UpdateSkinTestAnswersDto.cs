namespace BE_Team7.Dtos.SkinTestAnswers
{
    public class UpdateSkinTestAnswersDto
    {
        public Guid QuestionId { get; set; }
        public string AnswerDetail { get; set; }
        public int SkinNormalScore { get; set; }
        public int SkinDryScore { get; set; }
        public int SkinOilyScore { get; set; }
        public int SkinCombinationScore { get; set; }
        public int SkinSensitiveScore { get; set; }
    }
}
