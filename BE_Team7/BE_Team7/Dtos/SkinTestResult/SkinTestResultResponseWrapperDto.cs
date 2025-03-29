namespace BE_Team7.Dtos.SkinTestResult
{
    public class SkinTestResultResponseWrapperDto
    {
        public required string Name { get; set; }
        public int Age { get; set; }
        public required string Address { get; set; }
        public List<SkinTestResultResponseDto> SkinTestResultResponse { get; set; } = new();
    }
}
