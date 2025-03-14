using AutoMapper;
using BE_Team7.Dtos.FeedBack;
using BE_Team7.Dtos.ProductVariant;
using BE_Team7.Helpers;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace BE_Team7.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FeedbackRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<Feedback>> CreateFeedback(Feedback feedback)
        {
            var feedbackModel = await _context.Feedback.FirstOrDefaultAsync(x => x.ProductId == feedback.ProductId && x.Id == feedback.Id);
            if (feedbackModel != null)
            {
                return new ApiResponse<Feedback>
                {
                    Success = false,
                    Message = "Bạn đã đánh giá sản phẩm này",
                    Data = null
                };
            }

            // Gán giá trị CreatedAt
            feedback.CreatedAt = DateTime.UtcNow;

            _context.Feedback.Add(feedback);
            await _context.SaveChangesAsync();
            return new ApiResponse<Feedback>
            {
                Success = true,
                Message = "Tạo feedback thành công.",
                Data = feedbackModel
            };
        }

        public async Task<ApiResponse<Feedback>> DeleteFeedbackAsync(Guid feedbackId)
        {
            var feedbackModel = await _context.Feedback.FirstOrDefaultAsync(x => x.FeedbackId == feedbackId);
            if (feedbackModel == null)
            {
                return new ApiResponse<Feedback>
                {
                    Success = false,
                    Message = "Feedback không tồn tại.",
                    Data = null
                };
            }
            _context.Feedback.Remove(feedbackModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Feedback>
            {
                Success = true,
                Message = "Dalete Feedback thành công.",
                Data = feedbackModel
            };
        }

        public async Task<List<Feedback>> GetFeedbackAsync()
        {
            var feedback = _context.Feedback.AsQueryable();
            return await feedback.ToListAsync();
        }

        public async Task<Feedback?> GetFeedbackById(Guid feedbackId)
        {
            return await _context.Feedback.FirstOrDefaultAsync(i => i.FeedbackId == feedbackId);
        }

        public async Task<ApiResponse<Feedback>> UpdateFeedbackAsync(Guid feedbackId, UpdateFeedbackRequestDto updateFeedbackRequestDto)
        {
            var feedbackModel = await _context.Feedback.FirstOrDefaultAsync(x => x.FeedbackId == feedbackId);
            if (feedbackModel == null)
            {
                return new ApiResponse<Feedback>
                {
                    Success = false,
                    Message = "Feedback không tồn tại.",
                    Data = null
                };
            }
            _mapper.Map(updateFeedbackRequestDto, feedbackModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Feedback>
            {
                Success = true,
                Message = "Cập nhật Feedback thành công.",
                Data = feedbackModel
            };
        }
    }
}
