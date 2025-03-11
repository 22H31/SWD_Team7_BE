using BE_Team7.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_Team7.Interfaces
{
    public interface ISkinTestRepository
    {
        Task<IEnumerable<SkinTest>> GetAllQuestions();
        Task<SkinTest> GetQuestionById(Guid id);
        Task<SkinTest> AddQuestion(SkinTest question);
        Task<SkinTest> UpdateQuestion(Guid id, SkinTest question);
        Task<bool> DeleteQuestion(Guid id);
    }
}