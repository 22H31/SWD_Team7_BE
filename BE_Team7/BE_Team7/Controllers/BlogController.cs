using Microsoft.AspNetCore.Mvc;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Dtos.Blog;
using BE_Team7.Shared.ErrorModel;
using BE_Team7.Shared.Extensions;
using BE_Team7.Models;
using AutoMapper;
using BE_Team7.Dtos.Product;
using BE_Team7.Shared.ErrorModel;
using GarageManagementAPI.Shared.ResultModel;
using BE_Team7.Interfaces.Service.Contracts;

namespace BE_Team7.Controllers
{
    [Route("api/blogs")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBlogRepository _blogRepo;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;


        public BlogController(AppDbContext context, IBlogRepository blogRepo, IMapper mapper, IMediaService mediaService)
        {
            _blogRepo = blogRepo;
            _context = context;
            _mapper = mapper;
            _mediaService = mediaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlog()
        {
            var blogDtos = await _blogRepo.GetBlogsAsync();
            return Ok(blogDtos);
        }


        [HttpGet("{blogId}")]
        public async Task<IActionResult> GetBlogById([FromRoute] Guid blogId)
        {
            var blog = await _blogRepo.GetBlogById(blogId);
            if (blog == null)
            {
                return NotFound(new { message = "Blog không tồn tại." });
            }
            var blogDto = _mapper.Map<BlogDetailDto>(blog);
            return Ok(blogDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewBlog([FromBody] CreateBlogDto createBlogDto)
        {
            if (createBlogDto == null)
                return BadRequest("Invalid Blog data.");

            if (createBlogDto.Content2 == null ||
                createBlogDto.Content1 == null ||
                createBlogDto.Title == null)
            {
                return BadRequest("Incomplete Blog details.");
            }

            var blogModel = new Blog
            {
                Title = createBlogDto.Title,
                Content1 = createBlogDto.Content1,
                Content2 = createBlogDto.Content2,
                BlogCreatedAt = DateTime.Now,
                Id = createBlogDto.Id,
            };
            await _blogRepo.CreateBlogAsync(blogModel);
            return Ok(new { blogId = blogModel.BlogId });
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
        [HttpPost("{blogId:guid}/blog_images", Name = "CreateBlogImage")]
        public async Task<IActionResult> CreateBlogImage(Guid blogId, [FromForm] List<IFormFile> fileDtos)
        {
            var blogExists = await _blogRepo.GetBlogById(blogId);
            if (blogExists == null)
            {
                return NotFound(new { message = "Blog không tồn tại." });
            }
            if (fileDtos == null || !fileDtos.Any())
            {
                return BadRequest("No files were uploaded.");
            }
            var createdBlogImages = new List<object>();
            foreach (var fileDto in fileDtos)
            {
                var uploadFileResult = await _mediaService.UploadBlogImageAsync(fileDto);

                if (!uploadFileResult.IsSuccess)
                    return BadRequest(Result.Failure(HttpStatusCode.BadRequest, uploadFileResult.Errors));

                var imgTuple = uploadFileResult.GetValue<(string? publicId, string? absoluteUrl)>();

                var updateResult = await _blogRepo.CreateBlogImgAsync(blogId, imgTuple.publicId!, imgTuple.absoluteUrl!);

                if (!updateResult.Success) return BadRequest(Result.Failure(HttpStatusCode.BadRequest, new List<ErrorsResult>()));

            }
            return Ok();
        }
        [HttpPost("{blogId:guid}/blog_avartar_images", Name = "CreateblogAvartarImage")]
        public async Task<IActionResult> CreateBlogAvartarImage(Guid blogId, [FromForm] List<IFormFile> fileDtos)
        {
            var blogExists = await _blogRepo.GetBlogById(blogId);
            if (blogExists == null)
            {
                return NotFound(new { message = "Blog không tồn tại." });
            }
            if (fileDtos == null || !fileDtos.Any())
            {
                return BadRequest("No files were uploaded.");
            }
            var createdBlogAvartarImages = new List<object>();
            foreach (var fileDto in fileDtos)
            {
                var uploadFileResult = await _mediaService.UploadAvatarBlogImageAsync(fileDto);

                if (!uploadFileResult.IsSuccess)
                    return BadRequest(Result.Failure(HttpStatusCode.BadRequest, uploadFileResult.Errors));

                var imgTuple = uploadFileResult.GetValue<(string? publicId, string? absoluteUrl)>();

                var updateResult = await _blogRepo.CreateBlogAvartarImgAsync(blogId, imgTuple.publicId!, imgTuple.absoluteUrl!);

                if (!updateResult.Success) return BadRequest(Result.Failure(HttpStatusCode.BadRequest, new List<ErrorsResult>()));

            }
            return Ok();
        }
    }
}
