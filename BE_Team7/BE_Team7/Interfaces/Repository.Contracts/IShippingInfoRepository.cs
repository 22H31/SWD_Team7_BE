using BE_Team7.Dtos.ShippingInfo;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IShippingInfoRepository
    {
        Task<ApiResponse<ShippingInfo>> CreateShippingInfoAsync(ShippingInfoDto shippingInfoDto);
        Task<ApiResponse<ShippingInfo>> UpdateShippingInfoAsync(Guid shippingInfoId, UpdateShippingInfoDto shippingInfoDto);
        Task<ApiResponse<bool>> DeleteShippingInfoAsync(Guid shippingInfoId);
        Task<ApiResponse<List<ShippingInfo>>> GetShippingInfosByUserIdAsync(string id);
        Task<ApiResponse<bool>> UpdateDefaultAddressAsync(string id, Guid shippingInfoId);
        Task<ApiResponse<ShippingInfo>> GetShippingInfoByIdAsync(Guid shippingInfoId);
    }
}
