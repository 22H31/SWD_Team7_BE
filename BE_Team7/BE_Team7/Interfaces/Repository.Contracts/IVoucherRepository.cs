using BE_Team7.Dtos.Voucher;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<Voucher>> GetAllVouchersAsync();
        Task<Voucher> GetVoucherByIdAsync(Guid voucherId);
        Task<Voucher> AddVoucherAsync(Voucher voucher);
        Task<Voucher> UpdateVoucherAsync(Voucher voucher);
        Task<bool> DeleteVoucherAsync(Guid voucherId);
        Task<List<VoucherResponseDto>> GetVouchersByProductIdsAsync(List<Guid> productIds);
        Task<ApplyVoucherResponseDto> ApplyVoucherAsync(Guid voucherId);
    }
}
