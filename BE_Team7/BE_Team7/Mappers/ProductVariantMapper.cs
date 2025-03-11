using AutoMapper;
using BE_Team7.Dtos.Product;
using BE_Team7.Dtos.ProductVariant;
using BE_Team7.Models;

namespace BE_Team7.Mappers
{
    public class ProductVariantMapper : Profile
    {
        public ProductVariantMapper()
        {
            CreateMap<ProductVariant, ProductVariantDto>().ReverseMap();
            //map create
            CreateMap<ProductVariant, CreateProductVariantRequestDto>().ReverseMap();
            //map update
            CreateMap<UpdateProductVariantRequestDto, ProductVariant>()
            .ForMember(dest => dest.VariantId, opt => opt.Ignore());
        }
    }
}