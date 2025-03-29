using AutoMapper;
using BE_Team7.Dtos.SkinTestAnswers;
using BE_Team7.Dtos.SkinTestQuestion;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Team7.Controllers
{
    [Route("api/skintest-Answers")]
    [ApiController]
    public class SkinTestAnswersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISkinTestAnswersRepository _skinTestAnswersRepo;
        public SkinTestAnswersController(AppDbContext context, ISkinTestAnswersRepository skinTestAnswersRepo, IMapper mapper)
        {
            _skinTestAnswersRepo = skinTestAnswersRepo;
            _context = context;
            _mapper = mapper;
        }
        //[Authorize(Policy = "RequireAdminOrStaff")]
        [HttpGet("{answerId:Guid}")]
        public async Task<IActionResult> GetSkinTestAnswerById([FromRoute] Guid answerId)
        {
            try
            {
                var skinTestAnswer = await _skinTestAnswersRepo.GetSkinTestAnswersById(answerId);
                if (skinTestAnswer == null)
                {
                    return NotFound();
                }
                var skinTestAnswersDto = _mapper.Map<SkinTestAnswerDto>(skinTestAnswer);
                return Ok(skinTestAnswersDto);
            } 
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        //[Authorize(Policy = "RequireStaff")]
        [HttpPost]
        public async Task<IActionResult> CreateSkinTestAnswer([FromBody] CreateSkinTestAnswersDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newAnswer = new SkinTestAnswers
            {
                QuestionId = dto.QuestionId,
                AnswerDetail = dto.AnswerDetail,
                SkinNormalScore = dto.SkinNormalScore,
                SkinDryScore = dto.SkinDryScore,
                SkinOilyScore = dto.SkinOilyScore,
                SkinCombinationScore = dto.SkinCombinationScore,
                SkinSensitiveScore = dto.SkinSensitiveScore
            };

            try
            {
                var createdAnswer = await _skinTestAnswersRepo.CreateSkinTestAnswerAsync(newAnswer);
                return CreatedAtAction(nameof(GetSkinTestAnswerById), new { answerId = createdAnswer.AnswerId }, new SkinTestAnswerDto
                {
                    AnswerId = createdAnswer.AnswerId,
                    AnswerDetail = createdAnswer.AnswerDetail
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //[Authorize(Policy = "RequireStaff")]
        [HttpPut]
        [Route("{answerId:Guid}")]
        public async Task<IActionResult> UpdateCatregoryTitle([FromRoute] Guid answerId, [FromBody] UpdateSkinTestAnswersDto updateSkinTestAnswersDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<SkinTestAnswers>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",  
                Data = null
            }); ;
            var skinTestAnswersModel = await _skinTestAnswersRepo.UpdateSkinTestAnswersAsync(answerId, updateSkinTestAnswersDto);

            if (!skinTestAnswersModel.Success)
                return NotFound(skinTestAnswersModel);
            return Ok(skinTestAnswersModel);
        }
        //[Authorize(Policy = "RequireAdminOrStaff")]
        [HttpDelete("{answerId:Guid}")]
        public async Task<IActionResult> DeleteSkinTestQuestion([FromRoute] Guid answerId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<SkinTestAnswers>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var skinTestAnswersModel = await _skinTestAnswersRepo.DeleteSkinTestAnswersAsync(answerId);
            if (!skinTestAnswersModel.Success)
                return NotFound(skinTestAnswersModel);
            return Ok(skinTestAnswersModel);
        }   
    }
}
