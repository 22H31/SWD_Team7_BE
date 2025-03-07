using AutoMapper;
using BE_Team7.Dtos.Blog;
using BE_Team7.Dtos.Brand;
using BE_Team7.Models;

namespace BE_Team7.Mappers
{
    public class BlogMapper : Profile
    {
        public BlogMapper()
        {
            // Map giữa Brand và BrandDto
            CreateMap<Blog, BlogDto>().ReverseMap();
            //map create
            CreateMap<Blog, CreateBlogDto>().ReverseMap();
            //map update
            CreateMap<UpdateBlogDto, Blog>()
            .ForMember(dest => dest.BlogId, opt => opt.Ignore());
        }
    }
}
