using BE_Team7.Dtos.Blog;
using BE_Team7.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IBlogRepository
    {
        Task<List<Blog>> GetBlogsAsync();
        Task<Blog?> GetBlogById(Guid blogId);
        Task<ApiResponse<Blog>> CreateBlogAsync(Blog blog);
        Task<ApiResponse<Blog>> UpdateBlogAsync(Guid blogId, UpdateBlogDto updateBlogDto);
        Task<ApiResponse<Blog>> DeleteBlogAsync(Guid blogId);
    }
}
