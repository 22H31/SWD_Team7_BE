using BE_Team7.Dtos.ShippingInfo;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Repository
{
    public class ShippingInfoRepository : IShippingInfoRepository
    {
        private readonly AppDbContext _context;

        public ShippingInfoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<ShippingInfo>> CreateShippingInfoAsync(ShippingInfoDto shippingInfoDto)
        {
            try 
            { 
                // Kiểm tra xem người dùng đã có ShippingInfo nào chưa
                var hasExistingShippingInfo = await _context.ShippingInfo
                .AnyAsync(s => s.Id == shippingInfoDto.Id);
                
                var shippingInfo = new ShippingInfo
                {
                    Id = shippingInfoDto.Id,
                    AddressType = shippingInfoDto.AddressType,
                    LastName = shippingInfoDto.LastName,
                    FirstName = shippingInfoDto.FirstName,
                    ShippingPhoneNumber = shippingInfoDto.ShippingPhoneNumber,
                    Province = shippingInfoDto.Province,
                    District = shippingInfoDto.District,
                    Commune = shippingInfoDto.Commune,
                    AddressDetail = shippingInfoDto.AddressDetail,
                    ShippingNote = shippingInfoDto.ShippingNote,
                    DefaultAddress = !hasExistingShippingInfo,
                    ShippingInfoCreateAt = DateTime.UtcNow
                };

                _context.ShippingInfo.Add(shippingInfo);
                await _context.SaveChangesAsync();

                return new ApiResponse<ShippingInfo>
                {
                    Success = true,
                    Message = "ShippingInfo created successfully.",
                    Data = shippingInfo
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ShippingInfo>
                {
                    Success = false,
                    Message = $"Failed to create ShippingInfo: {ex.Message}",
                    Data = default
                };
            }
        }

        public async Task<ApiResponse<ShippingInfo>> UpdateShippingInfoAsync(Guid shippingInfoId, UpdateShippingInfoDto shippingInfoDto)
        {
            try
            {
                var shippingInfo = await _context.ShippingInfo.FindAsync(shippingInfoId);
                if (shippingInfo == null)
                {
                    return new ApiResponse<ShippingInfo>
                    {
                        Success = false,
                        Message = "ShippingInfo not found.",
                        Data = default
                    };
                }
                shippingInfo.AddressType = shippingInfoDto.AddressType;
                shippingInfo.LastName = shippingInfoDto.LastName;
                shippingInfo.FirstName = shippingInfoDto.FirstName;
                shippingInfo.ShippingPhoneNumber = shippingInfoDto.ShippingPhoneNumber;
                shippingInfo.Province = shippingInfoDto.Province;
                shippingInfo.District = shippingInfoDto.District;
                shippingInfo.Commune = shippingInfoDto.Commune;
                shippingInfo.AddressDetail = shippingInfoDto.AddressDetail;
                shippingInfo.ShippingNote = shippingInfoDto.ShippingNote;
                shippingInfo.DefaultAddress = shippingInfo.DefaultAddress;

                _context.ShippingInfo.Update(shippingInfo);
                await _context.SaveChangesAsync();

                return new ApiResponse<ShippingInfo>
                {
                    Success = true,
                    Message = "ShippingInfo updated successfully.",
                    Data = shippingInfo
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ShippingInfo>
                {
                    Success = false,
                    Message = $"Failed to update ShippingInfo: {ex.Message}",
                    Data = default
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteShippingInfoAsync(Guid shippingInfoId)
        {
            try
            {
                var shippingInfo = await _context.ShippingInfo.FindAsync(shippingInfoId);
                if (shippingInfo == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "ShippingInfo not found.",
                        Data = false
                    };
                }

                // Kiểm tra nếu là DefaultAddress thì không cho xóa
                if (shippingInfo.DefaultAddress)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Default address cannot be deleted. Please set another address as default before deleting.",
                        Data = false
                    };
                }

                _context.ShippingInfo.Remove(shippingInfo);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "ShippingInfo deleted successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Failed to delete ShippingInfo: {ex.Message}",
                    Data = false
                };
            }
        }
        public async Task<ApiResponse<List<ShippingInfo>>> GetShippingInfosByUserIdAsync(string id)
        {
            try
            {
                var shippingInfos = await _context.ShippingInfo
                    .Where(s => s.Id == id)
                    .ToListAsync();

                return new ApiResponse<List<ShippingInfo>>
                {
                    Success = true,
                    Message = "ShippingInfos retrieved successfully.",
                    Data = shippingInfos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ShippingInfo>>
                {
                    Success = false,
                    Message = $"Failed to retrieve ShippingInfos: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<bool>> UpdateDefaultAddressAsync(string id, Guid shippingInfoId)
        {
            try
            {
                // Tìm ShippingInfo hiện tại có DefaultAddress = true
                var currentDefault = await _context.ShippingInfo
                    .FirstOrDefaultAsync(s => s.Id == id && s.DefaultAddress);

                if (currentDefault != null)
                {
                    // Đổi DefaultAddress của ShippingInfo hiện tại thành false
                    currentDefault.DefaultAddress = false;
                    _context.ShippingInfo.Update(currentDefault);
                }

                // Tìm ShippingInfo mới và đặt DefaultAddress = true
                var newDefault = await _context.ShippingInfo
                    .FirstOrDefaultAsync(s => s.ShippingInfoId == shippingInfoId && s.Id == id);

                if (newDefault == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "ShippingInfo not found.",
                        Data = false
                    };
                }

                newDefault.DefaultAddress = true;
                _context.ShippingInfo.Update(newDefault);

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "DefaultAddress updated successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Failed to update DefaultAddress: {ex.Message}",
                    Data = false
                };
            }
        }
        public async Task<ApiResponse<ShippingInfo>> GetShippingInfoByIdAsync(Guid shippingInfoId)
        {
            try
            {
                var shippingInfo = await _context.ShippingInfo
                    .FirstOrDefaultAsync(s => s.ShippingInfoId == shippingInfoId);

                if (shippingInfo == null)
                {
                    return new ApiResponse<ShippingInfo>
                    {
                        Success = false,
                        Message = "ShippingInfo not found.",
                        Data = null
                    };
                }

                return new ApiResponse<ShippingInfo>
                {
                    Success = true,
                    Message = "ShippingInfo retrieved successfully.",
                    Data = shippingInfo
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ShippingInfo>
                {
                    Success = false,
                    Message = $"Error retrieving ShippingInfo: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}
