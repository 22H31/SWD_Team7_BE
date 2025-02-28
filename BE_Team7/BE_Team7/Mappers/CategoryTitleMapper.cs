using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.CategoryTitle;
using BE_Team7.Models;

namespace BE_Team7.Mappers
{
    public class CategoryTitleMapper : Profile
    {
        public CategoryTitleMapper()
        {
            // Map giữa CategoryTitle và CategoryTitleDto
            CreateMap<CategoryTitle, CategoryTitleDto>().ReverseMap();
            //map create
            CreateMap<CategoryTitle, CreateCategoryTitleRequestDto>().ReverseMap();
            //map update
            CreateMap<UpdateCategoryTitleRequestDto, CategoryTitle>()
            .ForMember(dest => dest.CategoryTitleId, opt => opt.Ignore());
        }
    }
}
