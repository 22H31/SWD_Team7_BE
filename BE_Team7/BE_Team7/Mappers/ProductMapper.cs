using BE_Team7.Models;
using AutoMapper;
using BE_Team7.Dtos.Product;

namespace BE_Team7.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt =>
            {
                opt.PreCondition(src => src.Category != null);
                opt.MapFrom(src => src.Category.CategoryName); // ✅ Map Category.Name vào ProductDto.CategoryName
            })
            .ReverseMap(); // Map từ Product → ProductDto
            CreateMap<Product, CreateProductRequestDto>().ReverseMap(); // Map từ Product → ProductDto
            CreateMap<UpdateProductRequestDto, Product>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore());
        }
    }
}
