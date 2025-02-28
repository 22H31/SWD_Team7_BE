using BE_Team7.Dtos.Category;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface ICategoryRepository
    {
        Task<List<GetCategoryTitleDto>> GetAllCategoriesAsync();
        Task<ApiResponse<Category>> CreateCategoryAsync(Category category);
        Task<ApiResponse<Category>> DeleteCategoryAsync(Guid categoryId);
        Task<Category?> GetCategoryById(Guid categoryId);
        Task<ApiResponse<Category>> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequestDto updateCategoryRequestDto);
    }
}
