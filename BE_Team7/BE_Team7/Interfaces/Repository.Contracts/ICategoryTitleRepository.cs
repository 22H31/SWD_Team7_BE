using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.CategoryTitle;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface ICategoryTitleRepository
    {
        Task<List<CategoryTitle>> GetCategoryAsync();
        Task<CategoryTitle?> GetCategoryTitleById(Guid categoryTitleId);
        Task<ApiResponse<CategoryTitle>> CreateCategoryTitleAsync(CategoryTitle categoryTitle);
        Task<ApiResponse<CategoryTitle>> UpdateCategoryTitleAsync(Guid categoryTitleId, UpdateCategoryTitleRequestDto updateCategoryTitleRequestDto);
        Task<ApiResponse<CategoryTitle>> DeleteCategoryTitleAsync(Guid categoryTitleId);
    }
}
