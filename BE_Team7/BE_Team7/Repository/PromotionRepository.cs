using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly AppDbContext _context;

        public PromotionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Promotion> CreatePromotionAsync(Promotion promotion)
        {
            _context.Promotion.Add(promotion);
            await _context.SaveChangesAsync();
            return promotion;
        }

        public async Task DeletePromotionAsync(Guid promotionId)
        {
            var promotion = await _context.Promotion.FirstOrDefaultAsync(p => p.PromotionId == promotionId);
            if (promotion != null)
            {
                _context.Promotion.Remove(promotion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            return await _context.Promotion.ToListAsync();
        }

        public async Task<Promotion> GetPromotionByCodeAsync(string promotionCode)
        {
            var promotion = await _context.Promotion.FirstOrDefaultAsync(p => p.PromotionCode == promotionCode);
            if (promotion == null)
            {
                throw new Exception("Promotion not found");
            }
            return promotion;
        }

        public async Task<Promotion> UpdatePromotionAsync(Guid promotionId, Promotion promotion)
        {
            var existingPromotion = await _context.Promotion.FindAsync(promotionId);
            if (existingPromotion != null)
            {
                existingPromotion.PromotionName = promotion.PromotionName;
                existingPromotion.PromotionCode = promotion.PromotionCode;
                existingPromotion.PromotionDescription = promotion.PromotionDescription;
                existingPromotion.DiscountRate = promotion.DiscountRate;
                existingPromotion.PromotionStartDate = promotion.PromotionStartDate;
                existingPromotion.PromotionEndDate = promotion.PromotionEndDate;

                await _context.SaveChangesAsync();
            }
            if (existingPromotion == null)
            {
                throw new Exception("Promotion not found");
            }
            return existingPromotion;
        }
    }
}
