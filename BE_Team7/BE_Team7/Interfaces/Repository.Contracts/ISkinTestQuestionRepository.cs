using BE_Team7.Dtos.CategoryTitle;
using BE_Team7.Dtos.SkinTestQuestion;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface ISkinTestQuestionRepository
    {
        Task<List<SkinTestQuestion>> GetSkinTestQuestionAsync();
        Task<SkinTestQuestion?> GetSkinTestQuestionById(Guid questionId);
        Task<ApiResponse<SkinTestQuestion>> CreateSkinTestQuestionAsync(SkinTestQuestion skinTestQuestion);
        Task<ApiResponse<SkinTestQuestion>> UpdateSkinTestQuestionAsync(Guid questionId, UpdateSkinTestQuestionDto updateSkinTestQuestionDto);
        Task<ApiResponse<SkinTestQuestion>> DeleteSkinTestQuestionAsync(Guid questionId);
    }
}
