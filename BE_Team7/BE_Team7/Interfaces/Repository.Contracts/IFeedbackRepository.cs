using BE_Team7.Dtos.FeedBack;
using BE_Team7.Helpers;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IFeedbackRepository
    {
        Task<List<Feedback>> GetFeedbackAsync();
        Task<Feedback?> GetFeedbackById(Guid feedbackId);
        //Task<List<Feedback>> GetFeeedbackByProductIdAsync(FeedbackQuery feedbackQuery, Guid productId);
        Task<ApiResponse<Feedback>> CreateFeedback(Feedback feedback);
        
    }
}
