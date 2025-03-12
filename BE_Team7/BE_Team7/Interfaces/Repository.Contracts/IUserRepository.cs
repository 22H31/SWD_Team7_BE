using BE_Team7.Dtos.Product;
using BE_Team7.Dtos.User;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserDetailDto?> GetUserByIdAsync(string Id);
        Task<ApiResponse<User>> UpdateUserById(Guid id, UpdateUserRequestDto user);
        Task<ApiResponse<User>> DeleteUserById(Guid id);
        Task<ApiResponse<AvatarImage>> UploadAvatarImage(string id, string publicId, string absoluteUrl);
    }
}
