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
