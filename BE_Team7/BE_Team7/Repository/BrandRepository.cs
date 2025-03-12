using AutoMapper;
using BE_Team7.Dtos.Brand;
using BE_Team7.Helpers;
using BE_Team7.Interfaces.Repository.Contracts;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Team7.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BrandRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<Brand>> CreateBrandAsync(Brand brand)
        {
            var brandModel = await _context.Brand.FirstOrDefaultAsync(x => x.BrandName == brand.BrandName);
            if (brandModel != null)
            {
                return new ApiResponse<Brand>
                {
                    Success = false,
                    Message = "Brand này đã tồn tại.",
                    Data = null
                };
            }
            _context.Brand.Add(brand);
            await _context.SaveChangesAsync();
            return new ApiResponse<Brand>
            {
                Success = true,
                Message = "Tạo sản phẩm thành công.",
                Data = brandModel
            };
        }   

        public async Task<List<Brand>> GetBrandAsync()
        {
            var brand = _context.Brand.AsQueryable();        
            return await brand.ToListAsync();
        }

        public async Task<Brand?> GetBrandById(string brandId)
        {
            return await _context.Brand.FirstOrDefaultAsync(i => i.BrandId.ToString() == brandId);
        }

        public async Task<ApiResponse<Brand>> UpdateBrandAsync(Guid brandId, UpdateBrandRequestDto updateBrandRequestDto)
        {
            var brandModel = await _context.Brand.FirstOrDefaultAsync(x => x.BrandId == brandId);
            if (brandModel == null)
            {
                return new ApiResponse<Brand>
                {
                    Success = false,
                    Message = "Brand không tồn tại.",
                    Data = null
                };
            }
            _mapper.Map(updateBrandRequestDto, brandModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Brand>
            {
                Success = true,
                Message = "Cập nhật brand thành công.",
                Data = brandModel
            };
        }
        public async Task<ApiResponse<Brand>> DeleteBrandAsync(Guid brandId)
        {
            var brandModel = await _context.Brand.FirstOrDefaultAsync(x => x.BrandId == brandId);
            if(brandModel == null)
            {
                return new ApiResponse<Brand>
                {
                    Success = false,
                    Message = "Brand không tồn tại",
                    Data = null
                };
            }
            _context.Brand.Remove(brandModel);
            await _context.SaveChangesAsync();
            return new ApiResponse<Brand>
            {
                Success = true,
                Message = "Xóa brand thành công",
                Data = brandModel
            };
        }
    }
}
