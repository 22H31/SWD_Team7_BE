using api.Interfaces;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Interfaces.Service.Contracts;
using BE_Team7.Shared.Extensions;
using BE_Team7.Shared.ErrorModel;
using GarageManagementAPI.Shared.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAccountRepository _acountRepo;
        private readonly IMediaService _mediaService;

        public UserController(AppDbContext context, IAccountRepository acountRepo, IMediaService mediaService)
        {
            _context = context;
            _acountRepo = acountRepo;
            _mediaService = mediaService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.User.ToListAsync();
            return Ok(users);
        }
        [HttpPost("{id:guid}/images", Name = "UploadAvaterImage")]
        public async Task<IActionResult> UploadAvatarImage(string id, [FromForm] List<IFormFile> fileDtos)
        {
            var acountExists = await _acountRepo.GetUserById(id);
            if (acountExists == null)
            {
                return NotFound(new { message = "Acount không tồn tại." });
            }
            if (fileDtos == null || !fileDtos.Any())
            {
                return BadRequest("No files were uploaded.");
            }
            var createdProductImages = new List<object>();
            foreach (var fileDto in fileDtos)
            {
                var uploadFileResult = await _mediaService.UploadProductImageAsync(fileDto);

                if (!uploadFileResult.IsSuccess)
                    return BadRequest(Result.Failure(HttpStatusCode.BadRequest, uploadFileResult.Errors));

                var imgTuple = uploadFileResult.GetValue<(string? publicId, string? absoluteUrl)>();

                var updateResult = await _acountRepo.UploadAvatarImage(id, imgTuple.publicId!, imgTuple.absoluteUrl!);

                if (!updateResult.Success) return BadRequest(Result.Failure(HttpStatusCode.BadRequest, new List<ErrorsResult>()));

            }
            return Ok();
        }
    }
}