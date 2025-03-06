namespace BE_Team7.Helpers
{
    public class ProductQuery
    {
        public string? Name {  get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
