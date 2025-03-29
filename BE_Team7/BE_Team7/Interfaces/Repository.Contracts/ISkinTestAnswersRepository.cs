using BE_Team7.Dtos.SkinTestAnswers;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface ISkinTestAnswersRepository
    {
        Task<List<SkinTestAnswers>> GetSkinTestAnswersAsync();
        Task<SkinTestAnswers?> GetSkinTestAnswersById(Guid answersId);
        Task<SkinTestAnswers> CreateSkinTestAnswerAsync(SkinTestAnswers answer);
        Task<ApiResponse<SkinTestAnswers>> UpdateSkinTestAnswersAsync(Guid answersId, UpdateSkinTestAnswersDto updateSkinTestAnswersDto);
        Task<ApiResponse<SkinTestAnswers>> DeleteSkinTestAnswersAsync(Guid answersId);
    }
}
