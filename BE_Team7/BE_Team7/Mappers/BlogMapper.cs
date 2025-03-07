using AutoMapper;
using BE_Team7.Dtos.Blog;
using BE_Team7.Models;

namespace BE_Team7.Mappers
{
    public class BlogMapper : Profile
    {
        public BlogMapper()
        {
            // Map Blog -> BlogDto and include Account Name
            CreateMap<Blog, BlogDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : "Unknown"));

            // Map CreateBlogDto -> Blog
            CreateMap<CreateBlogDto, Blog>().ReverseMap();

            // Map UpdateBlogDto -> Blog
            CreateMap<UpdateBlogDto, Blog>()
                .ForMember(dest => dest.BlogId, opt => opt.Ignore()); // Prevent overwriting ID
        }
    }
}
