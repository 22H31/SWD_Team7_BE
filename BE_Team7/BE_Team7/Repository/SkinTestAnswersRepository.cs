using AutoMapper;
using BE_Team7.Dtos.SkinTestAnswers;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Repository
{
    public class SkinTestAnswersRepository : ISkinTestAnswersRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SkinTestAnswersRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SkinTestAnswers> CreateSkinTestAnswerAsync(SkinTestAnswers answer)
        {
            var questionExists = await _context.SkinTestQuestions.AnyAsync(q => q.QuestionId == answer.QuestionId);
            if (!questionExists)
            {
                throw new ArgumentException("Invalid QuestionId: Question does not exist.");
            }

            _context.SkinTestAnswers.Add(answer);
            await _context.SaveChangesAsync();
            return answer;
        }

        public async Task<ApiResponse<SkinTestAnswers>> DeleteSkinTestAnswersAsync(Guid AnswerId)
        {
            var skinTestAnswersModel = await _context.SkinTestAnswers.FirstOrDefaultAsync(x => x.AnswerId == AnswerId);
            if (skinTestAnswersModel == null)
            {
                return new ApiResponse<SkinTestAnswers>
                {
                    Success = false,
                    Message = "Skin Test Answers không tồn tại",
                    Data = null
                };
            }
            _context.SkinTestAnswers.Remove(skinTestAnswersModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<SkinTestAnswers>
            {
                Success = true,
                Message = "Xóa Skin Test Answers thành công",
                Data = skinTestAnswersModel
            };
        }

        public async Task<List<SkinTestAnswers>> GetSkinTestAnswersAsync()
        {
            var categoryTitle = _context.SkinTestAnswers.AsQueryable();
            return await categoryTitle.ToListAsync();
        }

        public async Task<SkinTestAnswers?> GetSkinTestAnswersById(Guid AnswerId)
        {
            return await _context.SkinTestAnswers.FirstOrDefaultAsync(i => i.AnswerId == AnswerId);
        }

        public async Task<ApiResponse<SkinTestAnswers>> UpdateSkinTestAnswersAsync(Guid AnswerId, UpdateSkinTestAnswersDto updateSkinTestAnswersDto)
        {
            var skinTestAnswersModel = await _context.SkinTestAnswers.FirstOrDefaultAsync(x => x.AnswerId == AnswerId);
            if (skinTestAnswersModel == null)
            {
                return new ApiResponse<SkinTestAnswers>
                {
                    Success = false,
                    Message = "Skin Test Answers không tồn tại.",
                    Data = null
                };
            }
            _mapper.Map(updateSkinTestAnswersDto, skinTestAnswersModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<SkinTestAnswers>
            {
                Success = true,
                Message = "Cập nhật Skin Test Answers thành công.",
                Data = skinTestAnswersModel
            };
        }
    }
}
