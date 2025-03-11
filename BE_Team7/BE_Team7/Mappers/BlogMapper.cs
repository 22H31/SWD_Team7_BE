using AutoMapper;
using BE_Team7.Dtos.Blog;
using BE_Team7.Models;

namespace BE_Team7.Mappers
{
    public class BlogMapper : Profile
    {
        public BlogMapper()
        {
            CreateMap<Blog, BlogDetailDto>()
            .ForMember(dest => dest.BlogImageUrl, opt => opt.MapFrom(src => src.BlogImage
            .OrderByDescending(img => img.BlogImageCreatedAt) // Sắp xếp theo ngày tạo giảm dần
            .Take(2)
            .ToDictionary(img => $"img{src.BlogImage.ToList().IndexOf(img) + 1}", img => img.ImageUrl)))
            .ForMember(dest => dest.BlogAvartarImageUrl, opt => opt.MapFrom(src => src.BlogAvartarImage
            .OrderByDescending(img => img.BlogAvartarImageCreatedAt) // Sắp xếp theo thời gian tạo mới nhất
            .Select(img => img.ImageUrl) // Chỉ lấy URL ảnh
            .FirstOrDefault()));
            // Map CreateBlogDto -> Blog
            CreateMap<CreateBlogDto, Blog>().ReverseMap();
            // Map UpdateBlogDto -> Blog
            CreateMap<UpdateBlogDto, Blog>()
                .ForMember(dest => dest.BlogId, opt => opt.Ignore());
        }
    }
}
