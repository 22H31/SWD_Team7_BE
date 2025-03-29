using AutoMapper;
using BE_Team7.Dtos.SkinTestAnswers;
using BE_Team7.Dtos.SkinTestQuestion;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Controllers
{
    [Route("api/skintest-question")]
    [ApiController]
    public class SkinTestQuestionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISkinTestQuestionRepository _skinTestQuestionRepo;
        public SkinTestQuestionController(AppDbContext context, ISkinTestQuestionRepository skinTestQuestionRepo, IMapper mapper)
        {
            _skinTestQuestionRepo = skinTestQuestionRepo;
            _context = context;
            _mapper = mapper;
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestionsWithAnswers()
        {
            var questions = await _context.SkinTestQuestions
                .Select(q => new SkinTestQuestionDto
                {
                    QuestionId = q.QuestionId,
                    QuestionDetail = q.QuestionDetail,
                    Answers = _context.SkinTestAnswers
                        .Where(a => a.QuestionId == q.QuestionId)
                        .Select(a => new SkinTestAnswerDto
                        {
                            AnswerId = a.AnswerId,
                            AnswerDetail = a.AnswerDetail,
                            SkinNormalScore = a.SkinNormalScore,
                            SkinDryScore = a.SkinDryScore,
                            SkinOilyScore = a.SkinOilyScore,
                            SkinCombinationScore = a.SkinCombinationScore,
                            SkinSensitiveScore = a.SkinSensitiveScore
                        })  
                        .ToList()
                })
                .ToListAsync();
            return Ok(questions);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("{questionId:Guid}")]
        public async Task<IActionResult> GetSkinTestQuestionById([FromRoute] Guid questionId)
        {
            try
            {
                var skinTestQuestion = await _skinTestQuestionRepo.GetSkinTestQuestionById(questionId);
                if (skinTestQuestion == null)
                {
                    return NotFound();
                }
                var skinTestQuestionDto = _mapper.Map<SkinTestQuestionDto>(skinTestQuestion);
                return Ok(skinTestQuestionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        //[Authorize(Policy = "RequireStaff")]
        [HttpPost]
        public async Task<IActionResult> CreateNewSkinTestQuestion([FromBody] CreateSkinTestQuestionDto createSkinTestQuestionRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var skinTestQuestionModel = _mapper.Map<SkinTestQuestion>(createSkinTestQuestionRequestDto);
            await _skinTestQuestionRepo.CreateSkinTestQuestionAsync(skinTestQuestionModel);

            return CreatedAtAction(nameof(GetSkinTestQuestionById), new { questionId = skinTestQuestionModel.QuestionId }, _mapper.Map<SkinTestQuestionDto>(skinTestQuestionModel));
        }
        //[Authorize(Policy = "RequireStaff")]
        [HttpPut]
        [Route("{questionId:Guid}")]
        public async Task<IActionResult> UpdateCatregoryTitle([FromRoute] Guid questionId, [FromBody] UpdateSkinTestQuestionDto updateSkinTestQuestionRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Category>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var skinTestQuestionModel = await _skinTestQuestionRepo.UpdateSkinTestQuestionAsync(questionId, updateSkinTestQuestionRequestDto);

            if (!skinTestQuestionModel.Success)
                return NotFound(skinTestQuestionModel);
            return Ok(skinTestQuestionModel);
        }
        //[Authorize(Policy = "RequireAdminOrStaff")]
        [HttpDelete("{questionId:Guid}")]
        public async Task<IActionResult> DeleteSkinTestQuestion([FromRoute] Guid questionId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<SkinTestQuestion>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var skinTestQuestionModel = await _skinTestQuestionRepo.DeleteSkinTestQuestionAsync(questionId);
            if (!skinTestQuestionModel.Success)
                return NotFound(skinTestQuestionModel);
            return Ok(skinTestQuestionModel);
        }

    }
}
