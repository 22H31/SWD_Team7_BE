using BE_Team7.Dtos.Brand;
using BE_Team7.Helpers;
using BE_Team7.Models;

namespace BE_Team7.Interfaces.Repository.Contracts
{
    public interface IBrandRepository
    {
        Task<List<BrandDto>> GetBrandAsync();
        Task<Brand?> GetBrandById(string brandId);
        Task<ApiResponse<Brand>> CreateBrandAsync(Brand brand);
        Task<ApiResponse<Brand>> UpdateBrandAsync(Guid brandId, UpdateBrandRequestDto updateBrandRequestDto);
        Task<ApiResponse<Brand>> DeleteBrandAsync(Guid brandId);
        Task<ApiResponse<Brand>> UploadBrandAvartarImgAsync(Guid brandId, string publicId, string absoluteUrl);

    }
}
