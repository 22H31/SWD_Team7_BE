using AutoMapper;
using BE_Team7.Dtos.Blog;
using BE_Team7.Dtos.Brand;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace BE_Team7.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BlogRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BlogDto>> GetBlogsAsync()
        {
            var blogs = await _context.Blog
                .Include(b => b.User)
                .Include(b => b.BlogAvartarImage)
                .ToListAsync();
            var blogDtos = blogs.Select(b => new BlogDto
            {
                BlogId = b.BlogId,
                Title = b.Title,
                SubTitle = b.SubTitle,
                CreatedAt = DateTime.Now,
                Content1 = b.Content1, // Lấy tên user nếu có
                AvartarBlogUrl = b.BlogAvartarImage
                    .OrderByDescending(img => img.BlogAvartarImageCreatedAt) // Sắp xếp giảm dần theo ngày tạo
                    .Select(img => img.ImageUrl)
                    .FirstOrDefault() // Lấy ảnh mới nhất
            }).ToList();

            return blogDtos;
        }

        public async Task<Blog?> GetBlogById(Guid blogId)
        {
            return await _context.Blog
            .Include(p => p.BlogAvartarImage)
            .Include(p => p.BlogImage)
            .FirstOrDefaultAsync(p => p.BlogId == blogId);

        }
        public async Task<ApiResponse<Blog>> CreateBlogAsync(Blog blog)
        {
            var blogModel = await _context.Blog.FirstOrDefaultAsync(x => x.BlogId == blog.BlogId);
            if (blogModel != null)
            {
                return new ApiResponse<Blog>
                {
                    Success = false,
                    Message = "Blog này đã tồn tại.",
                    Data = null
                };
            }
            _context.Blog.Add(blog);
            await _context.SaveChangesAsync();
            return new ApiResponse<Blog>
            {
                Success = true,
                Message = "Tạo Blog thành công.",
                Data = blogModel
            };
        }



        public async Task<ApiResponse<Blog>> DeleteBlogAsync(Guid blogId)
        {
            var blogModel = await _context.Blog.FirstOrDefaultAsync(x => x.BlogId == blogId);
            if (blogModel == null)
            {
                return new ApiResponse<Blog>
                {
                    Success = false,
                    Message = "Blog không tồn tại",
                    Data = null
                };
            }
            _context.Blog.Remove(blogModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Blog>
            {
                Success = true,
                Message = "Xóa Blog thành công",
                Data = blogModel
            };
        }

        public async Task<ApiResponse<Blog>> UpdateBlogAsync(Guid blogId, UpdateBlogDto updateBlogDto)
        {
            var blogModel = await _context.Blog.FirstOrDefaultAsync(x => x.BlogId == blogId);
            if (blogModel == null)
            {
                return new ApiResponse<Blog>
                {
                    Success = false,
                    Message = "Blog không tồn tại.",
                    Data = null
                };
            }
            _mapper.Map(updateBlogDto, blogModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Blog>
            {
                Success = true,
                Message = "Cập nhật Blog thành công.",
                Data = blogModel
            };
        }

        public async Task<ApiResponse<Blog>> CreateBlogImgAsync(Guid blogId, string publicId, string absoluteUrl)
        {
            var blogImg = new BlogImage()
            {
                ImageUrl = absoluteUrl,
                ImageId = publicId,
                BlogId = blogId,
                BlogImageCreatedAt = DateTime.UtcNow
            };
            _context.BlogImage.Add(blogImg);
            await _context.SaveChangesAsync();

            return new ApiResponse<Blog>
            {
                Success = true,
                Message = "Upload thành công.",
                Data = null,
            };
        }

        public async Task<ApiResponse<Blog>> CreateBlogAvartarImgAsync(Guid blogId, string publicId, string absoluteUrl)
        {
            var blogAvartarImg = new BlogAvartarImage()
            {
                ImageUrl = absoluteUrl,
                ImageId = publicId,
                BlogId = blogId,
                BlogAvartarImageCreatedAt = DateTime.UtcNow
            };
            _context.BlogAvartarImage.Add(blogAvartarImg);
            await _context.SaveChangesAsync();

            return new ApiResponse<Blog>
            {
                Success = true,
                Message = "Upload thành công.",
                Data = null,
            };
        }
    }
}