using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.Product;
using BE_Team7.Models;

namespace BE_Team7.Mappers
{
    public class BrandMapper : Profile
    {
        public BrandMapper()
        {
            CreateMap<Brand, BrandDetailDto>()
             .ForMember(dest => dest.AvartarBrandUrl, opt => opt.MapFrom(src => src.BrandAvartarImage
            .OrderByDescending(img => img.BrandAvartarImageCreatedAt) // Sắp xếp theo thời gian tạo mới nhất
            .Select(img => img.ImageUrl) // Chỉ lấy URL ảnh
            .FirstOrDefault()));
            // Map giữa Brand và BrandDto
            CreateMap<Brand, BrandDto>().ReverseMap();
            //map create
            CreateMap<Brand, CreateBrandRequestDto>().ReverseMap();
            //map update
            CreateMap<UpdateBrandRequestDto, Brand>()
            .ForMember(dest => dest.BrandId, opt => opt.Ignore());
        }
    }
}
