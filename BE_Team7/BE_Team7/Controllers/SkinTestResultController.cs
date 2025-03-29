using BE_Team7.Dtos.SkinTestResult;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using BE_Team7.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Controllers
{
    [Route("api/Skin_Test_Result")]
    [ApiController]
    public class SkinTestResultController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISkinTestResultRepository _skinTestResultRepository;

        public SkinTestResultController(AppDbContext context, ISkinTestResultRepository skinTestResultRepository)
        {
            _context = context;
            _skinTestResultRepository = skinTestResultRepository;
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateSkinTestResult([FromBody] CreateSkinTestResultDto dto)
        {
            if (dto.AnswerIds == null || !dto.AnswerIds.Any())
            {
                return BadRequest("Danh sách câu trả lời không được để trống.");
            }

            // Lấy danh sách câu trả lời từ DB
            var answers = await _context.SkinTestAnswers
                .Where(a => dto.AnswerIds.Contains(a.AnswerId))
                .ToListAsync();

            if (!answers.Any())
            {
                return BadRequest("Không tìm thấy câu trả lời hợp lệ.");
            }

            // Tính tổng điểm cho từng loại da
            double totalSkinNormal = answers.Sum(a => a.SkinNormalScore);
            double totalSkinDry = answers.Sum(a => a.SkinDryScore);
            double totalSkinOily = answers.Sum(a => a.SkinOilyScore);
            double totalSkinCombination = answers.Sum(a => a.SkinCombinationScore);
            double totalSkinSensitive = answers.Sum(a => a.SkinSensitiveScore);

            // Xác định loại da có điểm cao nhất
            var skinScores = new Dictionary<string, double>
            {
                { "Normal", totalSkinNormal },
                { "Dry", totalSkinDry },
                { "Oily", totalSkinOily },
                { "Combination", totalSkinCombination },
                { "Sensitive", totalSkinSensitive }
            };

            string determinedSkinType = skinScores.OrderByDescending(s => s.Value).First().Key;

            // Tạo mới SkinTestResult
            var newResult = new SkinTestRerult
            {
                RerultId = Guid.NewGuid(),
                Id = dto.Id,
                TotalSkinNormalScore = totalSkinNormal,
                TotalSkinDryScore = totalSkinDry,
                TotalSkinOilyScore = totalSkinOily,
                TotalSkinCombinationScore = totalSkinCombination,
                TotalSkinSensitiveScore = totalSkinSensitive,
                SkinType = determinedSkinType,
                RerultCreateAt = DateTime.UtcNow
            };

            _context.RerultSkinTests.Add(newResult);
            await _context.SaveChangesAsync();

            return Ok(newResult);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("latest/{id}")]
        public async Task<IActionResult> GetLatestSkinTestResult(string id)
        {
            var result = await _skinTestResultRepository.GetLatestSkinTestResultByUserIdAsync(id);

            if (result == null)
            {
                return NotFound("User này chưa thực hiện bài test lần nào");
            }

            return Ok(result);
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpGet("{id}")]
        public async Task<ActionResult<SkinTestResultResponseWrapperDto>> GetSkinTestResults(string id)
        {
            var response = await _skinTestResultRepository.GetSkinTestResultResponseByUserIdAsync(id);

            if (response == null)
            {
                return NotFound("Không tìm thấy thông tin người dùng hoặc kết quả SkinTest.");
            }
            return Ok(response);
        }
    }
}
