using AutoMapper;
using BE_Team7.Dtos.CategoryTitle;
using BE_Team7.Dtos.SkinTestQuestion;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Repository
{
    public class SkinTestQuestionRepository : ISkinTestQuestionRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SkinTestQuestionRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<SkinTestQuestion>> CreateSkinTestQuestionAsync(SkinTestQuestion skinTestQuestion)
        {
            var skinTestQuestionModel = await _context.SkinTestQuestions.FirstOrDefaultAsync(x => x.QuestionDetail == skinTestQuestion.QuestionDetail);
            if (skinTestQuestionModel != null)
            {
                return new ApiResponse<SkinTestQuestion>
                {
                    Success = false,
                    Message = "Skin Test Question này đã tồn tại.",
                    Data = null
                };
            }
            _context.SkinTestQuestions.Add(skinTestQuestion);
            await _context.SaveChangesAsync();
            return new ApiResponse<SkinTestQuestion>
            {
                Success = true,
                Message = "Tạo Skin Test Question thành công.",
                Data = skinTestQuestionModel
            };
        }

        public async Task<ApiResponse<SkinTestQuestion>> DeleteSkinTestQuestionAsync(Guid questionId)
        {
            var skinTestQuestionModel = await _context.SkinTestQuestions.FirstOrDefaultAsync(x => x.QuestionId == questionId);
            if (skinTestQuestionModel == null)
            {
                return new ApiResponse<SkinTestQuestion>
                {
                    Success = false,
                    Message = "Skin Test Question không tồn tại",
                    Data = null
                };
            }
            _context.SkinTestQuestions.Remove(skinTestQuestionModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<SkinTestQuestion>
            {
                Success = true,
                Message = "Xóa Skin Test Question thành công",
                Data = skinTestQuestionModel
            };
        }

        public async Task<List<SkinTestQuestion>> GetSkinTestQuestionAsync()
        {
            var categoryTitle = _context.SkinTestQuestions.AsQueryable();
            return await categoryTitle.ToListAsync();
        }

        public async Task<SkinTestQuestion?> GetSkinTestQuestionById(Guid questionId)
        {
            return await _context.SkinTestQuestions.FirstOrDefaultAsync(i => i.QuestionId == questionId);
        }

        public async Task<ApiResponse<SkinTestQuestion>> UpdateSkinTestQuestionAsync(Guid questionId, UpdateSkinTestQuestionDto updateSkinTestQuestionDto)
        {
            var skinTestQuestionModel = await _context.SkinTestQuestions.FirstOrDefaultAsync(x => x.QuestionId == questionId);
            if (skinTestQuestionModel == null)
            {
                return new ApiResponse<SkinTestQuestion>
                {
                    Success = false,
                    Message = "Skin Test Question không tồn tại.",
                    Data = null
                };
            }
            _mapper.Map(updateSkinTestQuestionDto, skinTestQuestionModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<SkinTestQuestion>
            {
                Success = true,
                Message = "Cập nhật Skin Test Question thành công.",
                Data = skinTestQuestionModel
            };
        }
    }
}
