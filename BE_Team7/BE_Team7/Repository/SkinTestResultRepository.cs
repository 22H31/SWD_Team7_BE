using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Dtos.SkinTestResult;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Repository
{
    public class SkinTestResultRepository : ISkinTestResultRepository
    {
        private readonly AppDbContext _context;
        public SkinTestResultRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<SkinTestResultDetailDto?> GetLatestSkinTestResultByUserIdAsync(string id)
        {
            var latestResult = await _context.RerultSkinTests
                 .Where(r => r.Id == id)
                 .OrderByDescending(r => r.RerultCreateAt) 
                 .FirstOrDefaultAsync();
            if (latestResult == null)
            {
                return null;
            }

            return new SkinTestResultDetailDto
            {
                TotalSkinNormalScore = latestResult.TotalSkinNormalScore,
                TotalSkinDryScore = latestResult.TotalSkinDryScore,
                TotalSkinOilyScore = latestResult.TotalSkinOilyScore,
                TotalSkinCombinationScore = latestResult.TotalSkinCombinationScore,
                TotalSkinSensitiveScore = latestResult.TotalSkinSensitiveScore,
                SkinType = latestResult.SkinType,
                RerultCreateAt = latestResult.RerultCreateAt
            };
        }
        public async Task<ApiResponse<SkinTestResultResponseWrapperDto?>> GetSkinTestResultResponseByUserIdAsync(string id)
        {
            var user = await _context.Users
                 .Where(u => u.Id == id.ToString())
                 .Select(u => new
                 {
                     u.Name,
                     u.DateOfBirth,
                     u.Address
                 })
                 .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }
            var results = await _context.RerultSkinTests
                .Where(r => r.Id == id)
                .Select(r => new SkinTestResultResponseDto
                {
                    TotalSkinNormalScore = r.TotalSkinNormalScore,
                    TotalSkinDryScore = r.TotalSkinDryScore,
                    TotalSkinOilyScore = r.TotalSkinOilyScore,
                    TotalSkinCombinationScore = r.TotalSkinCombinationScore,
                    TotalSkinSensitiveScore = r.TotalSkinSensitiveScore,
                    SkinType = r.SkinType ?? "Unknown",
                    RerultCreateAt = r.RerultCreateAt,
                })
                .ToListAsync();

            return new ApiResponse<SkinTestResultResponseWrapperDto?>
            {
                Data = new SkinTestResultResponseWrapperDto
                {
                    Name = user?.Name ?? "Unknown",
                    Age = user != null ? (DateTime.Now.Year - user.DateOfBirth.Year) : 0,
                    Address = user?.Address ?? "Unknown",
                    SkinTestResultResponse = results
                },
                Message = "Lấy dữ liệu thành công",
                Success = true
            };
        }
    }
}
