namespace BE_Team7.Dtos.SkinTestResult
{
    public class SkinTestResultResponseDto
    {
        public double TotalSkinNormalScore { get; set; }
        public double TotalSkinDryScore { get; set; }   
        public double TotalSkinOilyScore { get; set; }
        public double TotalSkinCombinationScore { get; set; }
        public double TotalSkinSensitiveScore { get; set; }
        public required string SkinType { get; set; }
        public DateTime RerultCreateAt { get; set; }
    }
}
