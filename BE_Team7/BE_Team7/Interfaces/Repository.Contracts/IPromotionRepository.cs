using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IPromotionRepository
    {
        Task<Promotion> CreatePromotionAsync(Promotion promotion);
        Task DeletePromotionAsync(Guid promotionId);
        Task<Promotion> UpdatePromotionAsync(Guid promotionId, Promotion promotion);
        Task<Promotion> GetPromotionByCodeAsync(string promotionCode);
        Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
    }
}
