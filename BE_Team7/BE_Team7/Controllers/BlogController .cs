using Microsoft.AspNetCore.Mvc;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Dtos.Blog;
using System.Threading.Tasks;
using BE_Team7.Dtos.Brand;
using BE_Team7.Models;
using AutoMapper;

namespace BE_Team7.Controllers
{
    [Route("api/blogs")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBlogRepository _blogRepo;
        private readonly IMapper _mapper;


        public BlogController(AppDbContext context, IBlogRepository blogRepo, IMapper mapper)
        {
            _blogRepo = blogRepo;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlog()
        {
            var blogs = await _blogRepo.GetBlogsAsync();
            var blogDto = _mapper.Map<List<BlogDto>>(blogs);
            return Ok(blogDto);
        }

        [HttpGet("{blogId:Guid}")]
        public async Task<IActionResult> GetBlogById([FromRoute] Guid blogId)
        {
            try
            {
                var blog = await _blogRepo.GetBlogById(blogId);
                if (blog == null)
                {
                    return NotFound();
                }
                var blogDto = _mapper.Map<BlogDto>(blog);
                return Ok(blogDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewBlog([FromBody] CreateBlogDto createBlogDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var blogModel = _mapper.Map<Blog>(createBlogDto);
            await _blogRepo.CreateBlogAsync(blogModel);

            return CreatedAtAction(nameof(GetBlogById), new { id = blogModel.BlogId }, _mapper.Map<BlogDto>(blogModel));
        }

        [HttpPut("{blogId:Guid}")]
        public async Task<IActionResult> UpdateBlog([FromRoute] Guid blogId, [FromBody] UpdateBlogDto updateBlogDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Blog>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var blogModel = await _blogRepo.UpdateBlogAsync(blogId, updateBlogDto);

            if (!blogModel.Success)
                return NotFound(blogModel);
            return Ok(blogModel);
        }

        [HttpDelete("{blogId:Guid}")]
        public async Task<IActionResult> DeleteBlog([FromRoute] Guid blogId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Blog>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var blogModel = await _blogRepo.DeleteBlogAsync(blogId);
            if (!blogModel.Success)
                return NotFound(blogModel);
            return Ok(blogModel);
        }
    }
}
