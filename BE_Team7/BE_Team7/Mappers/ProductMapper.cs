using BE_Team7.Models;
using AutoMapper;
using BE_Team7.Dtos.Product;

namespace BE_Team7.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>().ReverseMap(); // Map từ Product → ProductDto
        }
    }
}
