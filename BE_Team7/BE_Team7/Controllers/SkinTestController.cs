using BE_Team7.DTOs;
using BE_Team7.Interfaces;
using BE_Team7.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_Team7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkinTestController : ControllerBase
    {
        private readonly ISkinTestRepository _skinTestRepository;

        public SkinTestController(ISkinTestRepository skinTestRepository)
        {
            _skinTestRepository = skinTestRepository;
        }

        // 1️⃣ Lấy toàn bộ câu hỏi
        [HttpGet("questions")]
        public async Task<ActionResult<IEnumerable<SkinTest>>> GetAllQuestions()
        {
            var questions = await _skinTestRepository.GetAllQuestions();
            return Ok(questions);
        }

        // 2️⃣ Lấy câu hỏi theo ID
        [HttpGet("questions/{id}")]
        public async Task<ActionResult<SkinTest>> GetQuestionById(Guid id)
        {
            var question = await _skinTestRepository.GetQuestionById(id);
            if (question == null)
            {
                return NotFound("Câu hỏi không tồn tại.");
            }
            return Ok(question);
        }

        // 3️⃣ Thêm câu hỏi mới
        [HttpPost("questions")]
        public async Task<ActionResult<SkinTest>> AddQuestion([FromBody] SkinTestQuestionDto questionDto)
        {
            var question = new SkinTest
            {
                QuestionDetail = questionDto.QuestionDetail,
                OptionA = questionDto.OptionA,
                OptionB = questionDto.OptionB,
                OptionC = questionDto.OptionC,
                OptionD = questionDto.OptionD
            };

            var createdQuestion = await _skinTestRepository.AddQuestion(question);
            return CreatedAtAction(nameof(GetQuestionById), new { id = createdQuestion.QuestionId }, createdQuestion);
        }

        // 4️⃣ Cập nhật câu hỏi
        [HttpPut("questions/{id}")]
        public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] SkinTestQuestionDto questionDto)
        {
            var updatedQuestion = new SkinTest
            {
                QuestionDetail = questionDto.QuestionDetail,
                OptionA = questionDto.OptionA,
                OptionB = questionDto.OptionB,
                OptionC = questionDto.OptionC,
                OptionD = questionDto.OptionD
            };

            var result = await _skinTestRepository.UpdateQuestion(id, updatedQuestion);
            if (result == null)
            {
                return NotFound("Câu hỏi không tồn tại.");
            }
            return Ok(result);
        }

        // 5️⃣ Xóa câu hỏi
        [HttpDelete("questions/{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var result = await _skinTestRepository.DeleteQuestion(id);
            if (!result)
            {
                return NotFound("Câu hỏi không tồn tại.");
            }
            return NoContent();
        }
    }
}