namespace BE_Team7.Dtos.Brand
{
    public class BrandDto
    {
        public Guid BrandId { get; set; }
        public required string BrandName { get; set; }
        public string AvartarBrandUrl { get; set; }
    }
}
