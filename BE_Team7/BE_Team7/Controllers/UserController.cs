using api.Interfaces;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Interfaces.Service.Contracts;
using BE_Team7.Shared.Extensions;
using BE_Team7.Shared.ErrorModel;
using GarageManagementAPI.Shared.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BE_Team7.Dtos.Product;
using BE_Team7.Dtos.User;
using api.Dtos.Account;
using Microsoft.AspNetCore.Identity;
using BE_Team7.Models;

namespace BE_Team7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly IAccountRepository _acountRepo;
        private readonly IMediaService _mediaService;
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, AppDbContext context, IAccountRepository acountRepo, IMediaService mediaService, IUserRepository userRepository, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _acountRepo = acountRepo;
            _mediaService = mediaService;
            _userRepository = userRepository;
            _roleManager = roleManager;
        }

        // GET: api/Users
        [HttpGet]
        //[Authorize(Roles = "Admin")] // Chỉ Admin mới có thể lấy danh sách
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        // API lấy thông tin chi tiết user
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound("User không tồn tại");
            return Ok(user);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateUserById([FromRoute] Guid id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        {   
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<User>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var userModel = await _userRepository.UpdateUserById(id, updateUserRequestDto);
            if (!userModel.Success)
                return NotFound(userModel);
            return Ok(userModel);
        }
        [HttpPost("create_staff_accout")]
        public async Task<IActionResult> CreateStaffAccout([FromBody] CreateUserRepositoryDto createUserRepositoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Kiểm tra email hoặc username đã tồn tại chưa
                if (await _userManager.FindByEmailAsync(createUserRepositoryDto.Email) != null)
                    return BadRequest("Email đã tồn tại.");

                if (await _userManager.FindByNameAsync(createUserRepositoryDto.UserName) != null)
                    return BadRequest("Username đã tồn tại.");

                // Tạo user mới
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = createUserRepositoryDto.UserName,
                    Email = createUserRepositoryDto.Email,
                    NormalizedUserName = createUserRepositoryDto.UserName.ToUpper(),
                    NormalizedEmail = createUserRepositoryDto.Email.ToUpper(),
                    EmailConfirmed = true, // Mặc định xác nhận email
                    SecurityStamp = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow
                };

                // Tạo tài khoản user
                var createdUser = await _userManager.CreateAsync(user, createUserRepositoryDto.Password);
                if (!createdUser.Succeeded)
                {
                    return StatusCode(500, createdUser.Errors);
                }

                // Kiểm tra role STAFF có tồn tại chưa, nếu chưa thì tạo
                string roleName = "STAFF";
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!createRoleResult.Succeeded)
                    {
                        return StatusCode(500, createRoleResult.Errors);
                    }
                }

                // Gán role STAFF cho user
                var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!roleResult.Succeeded)
                {
                    return StatusCode(500, roleResult.Errors);
                }

                return Ok(new { message = "Create successful", userId = user.Id });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpPost("change-role")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeRoleRepositoryDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound("User không tồn tại");

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Contains("Admin") || currentRoles.Contains("User"))
            {
                return BadRequest("Không thể thay đổi role của Admin hoặc User.");
            }

            string newRole = currentRoles.Contains("Staff") ? "StaffSale" : "Staff";

            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var result = await _userManager.AddToRoleAsync(user, newRole);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = $"Đổi role thành công, role mới: {newRole}" });
        }
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteProductVariant([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<User>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var userModel = await _userRepository.DeleteUserById(id);
            if (!userModel.Success)
                return NotFound(userModel);
            return Ok(userModel);
        }

        [HttpPost("{id}/images", Name = "UploadAvaterImage")]
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
                var uploadFileResult = await _mediaService.UploadAvatarImageAsync(fileDto);

                if (!uploadFileResult.IsSuccess)
                    return BadRequest(Result.Failure(HttpStatusCode.BadRequest, uploadFileResult.Errors));

                var imgTuple = uploadFileResult.GetValue<(string? publicId, string? absoluteUrl)>();

                var updateResult = await _userRepository.UploadAvatarImage(id, imgTuple.publicId!, imgTuple.absoluteUrl!);

                if (!updateResult.Success) return BadRequest(Result.Failure(HttpStatusCode.BadRequest, new List<ErrorsResult>()));

            }
            return Ok();
        }
    }
}