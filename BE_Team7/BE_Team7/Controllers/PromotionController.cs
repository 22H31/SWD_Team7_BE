using Microsoft.AspNetCore.Mvc;
using BE_Team7.Dtos;
using BE_Team7.Interfaces;
using BE_Team7.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Dtos.Promotion;
using Microsoft.AspNetCore.Authorization;

namespace BE_Team7.Controllers
{
    [Route("api/promotion")]
    [ApiController]
    public class PromotionController : Controller
    {
        private readonly IPromotionRepository _promotionRepository;

        public PromotionController(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }
        //[Authorize(Policy = "RequireStaffSaleOrAdmin")]
        [HttpGet("all")]
        public async Task<ApiResponse<IEnumerable<Promotion>>> GetAllPromotions()
        {
            try
            {
                var promotions = await _promotionRepository.GetAllPromotionsAsync();

                return new ApiResponse<IEnumerable<Promotion>>
                {
                    Success = true,
                    Message = "Lấy danh sách mã khuyến mãi thành công.",
                    Data = promotions
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<Promotion>>
                {
                    Success = false,
                    Message = $"Lỗi khi lấy danh sách mã khuyến mãi: {ex.Message}",
                    Data = null
                };
            }
        }
        //[Authorize(Policy = "RequireStaffSale")]
        [HttpPost("create")]
        public async Task<ApiResponse<Promotion>> CreatePromotion([FromBody] CreatePromotionDto dto)
        {
            try
            {
                var promotion = new Promotion
                {
                    PromotionName = dto.PromotionName,
                    PromotionCode = dto.PromotionCode,
                    PromotionDescription = dto.PromotionDescription,
                    DiscountRate = dto.DiscountRate,
                    PromotionStartDate = dto.PromotionStartDate,
                    PromotionEndDate = dto.PromotionEndDate
                };

                var createdPromotion = await _promotionRepository.CreatePromotionAsync(promotion);

                return new ApiResponse<Promotion>
                {
                    Success = true,
                    Message = "Tạo mã khuyến mãi thành công.",
                    Data = createdPromotion
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Promotion>
                {
                    Success = false,
                    Message = $"Lỗi khi tạo mã khuyến mãi: {ex.Message}",
                    Data = null
                };
            }
        }
        //[Authorize(Policy = "RequireStaffSaleOrAdmin")]
        [HttpDelete("delete/{promotionId}")]
        public async Task<ApiResponse<string>> DeletePromotion(Guid promotionId)
        {
            try
            {
                await _promotionRepository.DeletePromotionAsync(promotionId);

                return new ApiResponse<string>
                {
                    Success = true,
                    Message = "Xóa mã khuyến mãi thành công.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Lỗi khi xóa mã khuyến mãi: {ex.Message}",
                    Data = null
                };
            }
        }
        //[Authorize(Policy = "RequireStaffSale")]
        [HttpPut("update/{promotionId}")]
        public async Task<ApiResponse<Promotion>> UpdatePromotion(Guid promotionId, [FromBody] Promotion promotion)
        {
            try
            {
                var updatedPromotion = await _promotionRepository.UpdatePromotionAsync(promotionId, promotion);

                if (updatedPromotion == null)
                {
                    return new ApiResponse<Promotion>
                    {
                        Success = false,
                        Message = "Không tìm thấy mã khuyến mãi.",
                        Data = null
                    };
                }

                return new ApiResponse<Promotion>
                {
                    Success = true,
                    Message = "Cập nhật mã khuyến mãi thành công.",
                    Data = updatedPromotion
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Promotion>
                {
                    Success = false,
                    Message = $"Lỗi khi cập nhật mã khuyến mãi: {ex.Message}",
                    Data = null
                };
            }
        }
        //[Authorize(Policy = "RequireAlll")]
        [HttpPost("apply")]
        public async Task<ApiResponse<PromotionResponseDto>> ApplyPromotion([FromBody] ApplyPromotionDto dto)
        {
            try
            {
                var promotion = await _promotionRepository.GetPromotionByCodeAsync(dto.PromotionCode);

                if (promotion == null)
                {
                    return new ApiResponse<PromotionResponseDto>
                    {
                        Success = false,
                        Message = "Mã khuyến mãi không tồn tại.",
                        Data = null
                    };
                }

                if (DateTime.Now < promotion.PromotionStartDate || DateTime.Now > promotion.PromotionEndDate)
                {
                    return new ApiResponse<PromotionResponseDto>
                    {
                        Success = false,
                        Message = "Mã khuyến mãi đã hết hạn hoặc chưa có hiệu lực.",
                        Data = null
                    };
                }

                var response = new PromotionResponseDto
                {
                    PromotionId = promotion.PromotionId,
                    DiscountRate = promotion.DiscountRate,
                    Message = "Áp dụng mã khuyến mãi thành công."
                };

                return new ApiResponse<PromotionResponseDto>
                {
                    Success = true,
                    Message = "Áp dụng mã khuyến mãi thành công.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PromotionResponseDto>
                {
                    Success = false,
                    Message = $"Lỗi khi áp dụng mã khuyến mãi: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}
