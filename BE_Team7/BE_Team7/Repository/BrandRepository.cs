using AutoMapper;
using BE_Team7.Dtos.Blog;
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

        public async Task<List<BrandDto>> GetBrandAsync()
        {
            var brands = await _context.Brand
            .Include(b => b.BrandAvartarImage)
            .ToListAsync();
            var brandDtos = brands.Select(b => new BrandDto
            {
                BrandId = b.BrandId,
                BrandName = b.BrandName,
                AvartarBrandUrl = b.BrandAvartarImage
                    .OrderByDescending(img => img.BrandAvartarImageCreatedAt)
                    .Select(img => img.ImageUrl)
                    .FirstOrDefault()
            }).ToList();

            return brandDtos;
        }

        public async Task<Brand?> GetBrandById(string brandId)
        {
            return await _context.Brand
            .Include(p => p.BrandAvartarImage)
            .FirstOrDefaultAsync(p => p.BrandId.ToString() == brandId);
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

        public async Task<ApiResponse<Brand>> UploadBrandAvartarImgAsync(Guid brandId, string publicId, string absoluteUrl)
        {
            var brandAvartarImg = new BrandAvartarImage()
            {
                ImageUrl = absoluteUrl,
                ImageId = publicId,
                BrandId = brandId,
                BrandAvartarImageCreatedAt = DateTime.UtcNow
            };
            _context.BrandAvartarImage.Add(brandAvartarImg);
            await _context.SaveChangesAsync();

            return new ApiResponse<Brand>
            {
                Success = true,
                Message = "Upload thành công.",
                Data = null,
            };
        }
    }
}
