using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Dtos.FeedBack;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
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
        [HttpGet]
        public async Task<IActionResult> GetAllBrand()
        {
            var feedbacks = await _feedbackRepo.GetFeedbackAsync();
            var feedbackDto = _mapper.Map<List<FeedbackDto>>(feedbacks);
            return Ok(feedbacks);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewFeedback([FromBody] CreateFeebackRequestDto createFeebackRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var feedbackModel = _mapper.Map<Feedback>(createFeebackRequestDto);
            await _feedbackRepo.CreateFeedback(feedbackModel);

            return CreatedAtAction(nameof(GetFeedbackById), new { id = feedbackModel.FeedbackId }, _mapper.Map<FeedbackDto>(feedbackModel));
        }

        [HttpGet("{feedbackId:Guid}")]
        public async Task<IActionResult> GetFeedbackById([FromRoute] Guid feedbackId)
        {
            try
            {
                var feedback = await _feedbackRepo.GetFeedbackById(feedbackId);
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
    }
}
