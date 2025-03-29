namespace BE_Team7.Dtos.SkinTestResult
{
    public class CreateSkinTestResultDto
    {
        public string Id { get; set; }
        public List<Guid> AnswerIds { get; set; } = new();
    }
}
