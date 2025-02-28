using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.Category;
using BE_Team7.Models;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        //map craete
        CreateMap<Category, CreateCategoryRequestDto>().ReverseMap();
        // map tu thuong sang Dto
        CreateMap<Category, CategoryDto>();
        // map lay ra  toan bo
        CreateMap<CategoryTitle, GetCategoryTitleDto>()
            .ForMember(dest => dest.Categorys, opt => opt.MapFrom(src => src.Category));
        //map update
        CreateMap<UpdateCategoryRequestDto, Category>()
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore());
    }
}