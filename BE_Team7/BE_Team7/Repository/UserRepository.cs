using AutoMapper;
using BE_Team7;
using BE_Team7.Dtos.User;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserRepository(UserManager<User> userManager, IMapper mapper, AppDbContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    public async Task<ApiResponse<User>> DeleteUserById(Guid id)
    {
        var userModel = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (userModel == null)
        {
            return new ApiResponse<User>
            {
                Success = false,
                Message = "User không tồn tại",
                Data = null
            };
        }

        var result = await _userManager.DeleteAsync(userModel); // Dùng UserManager để xóa user

        if (!result.Succeeded)
        {
            return new ApiResponse<User>
            {
                Success = false,
                Message = "Xóa user thất bại",
                Data = null
            };
        }

        return new ApiResponse<User>
        {
            Success = true,
            Message = "Xóa User thành công",
            Data = userModel
        };
    }

    // Lấy tất cả user
    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.Include(u => u.AvatarImages).ToListAsync();

        var userDtos = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                LockoutEnd = user.LockoutEnd ?? DateTimeOffset.MinValue,
                Name = user.UserName,
                Avatar = user.AvatarImages?.FirstOrDefault()?.ImageUrl,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                CreatedAt = user.CreatedAt,
                Roles = roles.ToList() // Gán danh sách role
            };
            userDtos.Add(userDto);
        }
        return userDtos;
    }
    // Lấy chi tiết user
    public async Task<UserDetailDto?> GetUserByIdAsync(string id)
    {
        var user = await _userManager.Users
        .Include(u => u.AvatarImages)
        .Include(u => u.RerultSkinTest)
        .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return null;

        // Lấy avatar mới nhất (nếu có)
        var latestAvatar = user.AvatarImages?
            .OrderByDescending(img => img.AvatarImageCreatedAt)
            .FirstOrDefault()?.ImageUrl;

        // Lấy kiểu da mới nhất (nếu có)
        var latestSkinType = user.RerultSkinTest?
            .OrderByDescending(test => test.RerultCreateAt)
            .FirstOrDefault()?.SkinType;

        // Trả về DTO với giá trị đã xử lý
        return new UserDetailDto
        {
            Avatar = latestAvatar,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            SkinType = latestSkinType,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<ApiResponse<User>> UpdateUserById(Guid id, UpdateUserRequestDto updateUserRequestDto)
    {
        var userModel = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.ToString() == id.ToString());
        if (userModel == null) 
        {
            return new ApiResponse<User>
            {
                Success = false,
                Message = "User không tồn tại.",
                Data = null
            };
        }
        _mapper.Map(updateUserRequestDto, userModel);
        var result = await _userManager.UpdateAsync(userModel);
        if (!result.Succeeded)
        {
            return new ApiResponse<User>
            {
                Success = false,
                Message = "Cập nhật user thất bại.",
                Data = null
            };
        }
        await _context.SaveChangesAsync();
        return new ApiResponse<User>
        {
            Success = true,
            Message = "Cập nhật user thành công.",
            Data = userModel
        };
    }

    public async Task<ApiResponse<AvatarImage>> UploadAvatarImage(string id, string publicId, string absoluteUrl)
    {

        var avatarImg = new AvatarImage()
        {
            ImageUrl = absoluteUrl,
            ImageId = publicId,
            Id = id,
            AvatarImageCreatedAt = DateTime.Now,

        };
        _context.AvatarImage.Add(avatarImg);
        await _context.SaveChangesAsync();

        return new ApiResponse<AvatarImage>
        {
            Success = true,
            Message = "upload avater thành công.",
            Data = avatarImg,
        };
    }

    //// Gán Role cho User
    //public async Task<bool> AssignRoleAsync(string userId, string role)
    //{
    //    var user = await _userManager.FindByIdAsync(userId);
    //    if (user == null) return false;

    //    var result = await _userManager.AddToRoleAsync(user, role);
    //    return result.Succeeded;
    //}

    //// Xóa Role của User
    //public async Task<bool> RemoveRoleAsync(string userId, string role)
    //{
    //    var user = await _userManager.FindByIdAsync(userId);
    //    if (user == null) return false;

    //    var result = await _userManager.RemoveFromRoleAsync(user, role);
    //    return result.Succeeded;
    //}
}
