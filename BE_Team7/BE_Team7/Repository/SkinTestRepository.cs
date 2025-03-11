using BE_Team7.Interfaces;
using BE_Team7.Models;
using BE_Team7.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_Team7.Repositories
{
    public class SkinTestRepository : ISkinTestRepository
    {
        private readonly AppDbContext _context;

        public SkinTestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SkinTest>> GetAllQuestions()
        {
            return await _context.SkinTest.ToListAsync();
        }

        public async Task<SkinTest> GetQuestionById(Guid id)
        {
            return await _context.SkinTest.FindAsync(id);
        }

        public async Task<SkinTest> AddQuestion(SkinTest question)
        {
            _context.SkinTest.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        public async Task<SkinTest> UpdateQuestion(Guid id, SkinTest updatedQuestion)
        {
            var existingQuestion = await _context.SkinTest.FindAsync(id);
            if (existingQuestion == null) return null;

            existingQuestion.QuestionDetail = updatedQuestion.QuestionDetail;
            existingQuestion.OptionA = updatedQuestion.OptionA;
            existingQuestion.OptionB = updatedQuestion.OptionB;
            existingQuestion.OptionC = updatedQuestion.OptionC;
            existingQuestion.OptionD = updatedQuestion.OptionD;

            await _context.SaveChangesAsync();
            return existingQuestion;
        }

        public async Task<bool> DeleteQuestion(Guid id)
        {
            var question = await _context.SkinTest.FindAsync(id);
            if (question == null) return false;

            _context.SkinTest.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}