using BE_Team7.Dtos.Voucher;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly AppDbContext _context;

        public VoucherRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Voucher> AddVoucherAsync(Voucher voucher)
        {
            if (voucher == null)
            {
                throw new ArgumentNullException(nameof(voucher), "Dữ liệu voucher không được null.");
            }

            bool exists = await _context.Voucher.AnyAsync(v => v.VoucherId == voucher.VoucherId);
            if (exists)
            {
                throw new KeyNotFoundException($"Voucher với ID {voucher.VoucherId} đã tồn tại.");
            }

            _context.Voucher.Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task<bool> DeleteVoucherAsync(Guid voucherId)
        {
            var voucher = await _context.Voucher.FindAsync(voucherId);
            if (voucher == null)
            {
                return false;
            }
            _context.Voucher.Remove(voucher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Voucher>> GetAllVouchersAsync()
        {
            return await _context.Voucher.ToListAsync();
        }

        public async Task<Voucher> GetVoucherByIdAsync(Guid voucherId)
        {
            var voucher = await _context.Voucher.FindAsync(voucherId);
            if (voucher == null)
            {
                throw new KeyNotFoundException($"Voucher với ID {voucherId} không tồn tại.");
            }
            return voucher;
        }

        public async Task<List<VoucherResponseDto>> GetVouchersByProductIdsAsync(List<Guid> productIds)
        {
            var products = await _context.Products
                .Where(P => productIds.Contains(P.ProductId))
                .ToListAsync();
            var brandIds = products.Select(p => p.BrandId).Distinct().ToList();
            var categoryIds  = products.Select(p => p.CategoryId).Distinct().ToList();

            var vouchers = await _context.Voucher
                .Where(v => (v.BrandId.HasValue && brandIds.Contains(v.BrandId.Value)) || (v.CategoryId.HasValue && categoryIds.Contains(v.CategoryId.Value)) &&
                    v.VoucherQuantity > 0 &&
                    v.VoucherStartDate <= DateTime.UtcNow &&
                    v.VoucherEndDate >= DateTime.UtcNow)
                .Select(v => new VoucherResponseDto
                {
                    VoucherId = v.VoucherId,
                    VoucherName = v.VoucherName,
                    VoucherDescription = v.VoucherDescription,
                    VoucherEndDate = v.VoucherEndDate,
                    VoucherQuantity = v.VoucherQuantity,
                    VoucherRate = v.VoucherRate
                })
                .ToListAsync();
            return vouchers;
        }

        public async Task<Voucher> UpdateVoucherAsync(Voucher voucher)
        {
            _context.Entry(voucher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task<ApplyVoucherResponseDto> ApplyVoucherAsync(Guid voucherId)
        {
            // Lấy thông tin voucher từ voucherId
            var voucher = await _context.Voucher
                .FirstOrDefaultAsync(v => v.VoucherId == voucherId);

            if (voucher == null)
            {
                throw new KeyNotFoundException("Voucher not found.");
            }

            // Lấy danh sách ProductId có BrandId hoặc CategoryId trùng với voucher
            var applicableProductIds = await _context.Products
                .Where(p => (voucher.BrandId.HasValue && p.BrandId == voucher.BrandId.Value) ||
                            (voucher.CategoryId.HasValue && p.CategoryId == voucher.CategoryId.Value))
                .Select(p => p.ProductId)
                .ToListAsync();

            // Tạo response
            var response = new ApplyVoucherResponseDto
            {
                VoucherId = voucherId,
                VoucherName = voucher.VoucherName,
                VoucherRate = voucher.VoucherRate,
                ApplicableProductIds = applicableProductIds
            };

            return response;
        }
    }
}
