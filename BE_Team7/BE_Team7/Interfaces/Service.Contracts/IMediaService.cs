using static BE_Team7.Shared.Result;

namespace BE_Team7.Interfaces.Service.Contracts
{
    public interface IMediaService
    {
        Task<Result<(string? publicId, string? absoluteUrl)>> UploadUserImageAsync(IFormFile file);
        Task<Result<(string? publicId, string? absoluteUrl)>> UploadProductImageAsync(IFormFile file);
        Task<Result<(string? publicId, string? absoluteUrl)>> UploadServiceImageAsync(IFormFile file);
        Task<Result<(string? publicId, string? absoluteUrl)>> UploadBrandImageAsync(IFormFile file);
        Result<string> RemoveImage(string publicId);
    }
}
