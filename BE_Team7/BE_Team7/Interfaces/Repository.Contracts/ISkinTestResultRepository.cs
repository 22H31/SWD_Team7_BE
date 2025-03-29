using BE_Team7.Dtos.SkinTestResult;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface ISkinTestResultRepository
    {
        Task<SkinTestResultDetailDto?> GetLatestSkinTestResultByUserIdAsync(string id);
        Task<ApiResponse<SkinTestResultResponseWrapperDto?>> GetSkinTestResultResponseByUserIdAsync(string id);
    }
}
