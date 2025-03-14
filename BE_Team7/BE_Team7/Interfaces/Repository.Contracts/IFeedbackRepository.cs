using BE_Team7.Dtos.FeedBack;
using BE_Team7.Dtos.ProductVariant;
using BE_Team7.Helpers;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IFeedbackRepository
    {
        Task<List<Feedback>> GetFeedbackAsync();
        Task<Feedback?> GetFeedbackById(Guid feedbackId);
        Task<ApiResponse<Feedback>> CreateFeedback(Feedback feedback);
        Task<ApiResponse<Feedback>> DeleteFeedbackAsync(Guid feedbackId);
        Task<ApiResponse<Feedback>> UpdateFeedbackAsync(Guid feedbackId, UpdateFeedbackRequestDto updateFeedbackRequestDto);
    }
}
