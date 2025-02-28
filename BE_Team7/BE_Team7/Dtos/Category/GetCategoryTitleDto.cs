namespace BE_Team7.Dtos.Category
{
    public class GetCategoryTitleDto
    {
        public Guid CategoryTitleId { get; set; }
        public string CategoryTitleName { get; set; } = string.Empty;
        public string CategoryTitleIcon { get; set; } = string.Empty;
        public List<CategoryDto> Categorys { get; set; } = new();
    }
}
