using AutoMapper;
using BE_Team7.Dtos.FeedBack;
using BE_Team7.Dtos.ProductVariant;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_Team7.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFeedbackRepository _feedbackRepo;
        public FeedbackController(AppDbContext context, IFeedbackRepository feedbackRepo, IMapper mapper)
        {
            _feedbackRepo = feedbackRepo;
            _context = context;
            _mapper = mapper;
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet]
        public async Task<IActionResult> GetAllBrand()
        {
            var feedbacks = await _feedbackRepo.GetFeedbackAsync();
            var feedbackDto = _mapper.Map<List<FeedbackDto>>(feedbacks);
            return Ok(feedbacks);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpPost]
        public async Task<IActionResult> CreateNewFeedback([FromBody] CreateFeebackRequestDto createFeebackRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var feedbackModel = _mapper.Map<Feedback>(createFeebackRequestDto);
            await _feedbackRepo.CreateFeedback(feedbackModel);

            return CreatedAtAction(nameof(GetFeedbackById), new { id = feedbackModel.FeedbackId }, _mapper.Map<FeedbackDto>(feedbackModel));
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedbackById([FromRoute] Guid id)
        {
            try
            {
                var feedback = await _feedbackRepo.GetFeedbackById(id);
                if (feedback == null)
                {
                    return NotFound();
                }
                var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
                return Ok(feedbackDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpPut]
        [Route("{feedbackId:Guid}")]
        public async Task<IActionResult> UpdateProductVariant([FromRoute] Guid feedbackId, [FromBody] UpdateFeedbackRequestDto updateFeedbackRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Feedback>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var feedbackModel = await _feedbackRepo.UpdateFeedbackAsync(feedbackId, updateFeedbackRequestDto);

            if (!feedbackModel.Success)
                return NotFound(feedbackModel);
            return Ok(feedbackModel);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpDelete("{feedbackId:Guid}")]
        public async Task<IActionResult> DeleteProductVariant([FromRoute] Guid feedbackId)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponse<Feedback>
            {
                Success = false,
                Message = "Dữ liệu không hợp lệ.",
                Data = null
            }); ;
            var feedbackModel = await _feedbackRepo.DeleteFeedbackAsync(feedbackId);
            if (!feedbackModel.Success)
                return NotFound(feedbackModel);
            return Ok(feedbackModel);
        }
    }
}
