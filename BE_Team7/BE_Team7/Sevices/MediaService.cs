using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using GarageManagementAPI.Shared.ResultModel;
using BE_Team7.Dtos;
using BE_Team7.Shared.Contanst;
using BE_Team7.Interfaces.Service.Contracts;

namespace GarageManagementAPI.Service
{
    public class MediaService : IMediaService
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettings _configuration;
        private readonly long _maxFileSize = 10 * 1024 * 1024; // 10 MB

        private static readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png" };
        private static readonly string[] _permittedMimeTypes = { "image/jpg", "image/jpeg", "image/png" };

        private const string _userFolder = "User";
        private const string _productFolder = "Product";
        private const string _productAvartarFolder = "ProductAvartar";
        private const string _blogFolder = "Blog";
        private const string _blogAvartarFolder = "BlogAvartar";
        private const string _errorCode = "CloudinaryError";

        public MediaService(IOptionsSnapshot<CloudinarySettings> configuration)
        {
            _configuration = configuration.Value;
            var account = new Account(
                _configuration.CloudName,
                _configuration.ApiKey,
                _configuration.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        private async Task<Result<(string? publicId, string? absoluteUrl)>> UploadImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                return Result<(string? publicId, string? absoluteUrl)>.BadRequest([RequestErrors.GetFileNotFoundErrors()]);
            }

            if (file.Length > _maxFileSize)
            {
                return Result<(string? publicId, string? absoluteUrl)>.BadRequest([RequestErrors.GetFileTooLargeErrors()]);
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !_permittedExtensions.Contains(extension))
            {
                return Result<(string? publicId, string? absoluteUrl)>.BadRequest([RequestErrors.GetFileExtensionInvalidErrors()]);
            }

            if (!_permittedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                return Result<(string? publicId, string? absoluteUrl)>.BadRequest([RequestErrors.GetFileTypeInvalidErrors()]);
            }

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                UniqueFilename = true,
                Overwrite = true,
                AssetFolder = folderName,
                UseAssetFolderAsPublicIdPrefix = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if ((int)uploadResult.StatusCode == (int)HttpStatusCode.OK)
            {
                return Result<(string? publicId, string? absoluteUrl)>.Ok((publicId: uploadResult.PublicId, absoluteUrl: uploadResult.SecureUrl.AbsoluteUri));
            }

            return Result<(string? publicId, string? absoluteUrl)>.Failure((HttpStatusCode)uploadResult.StatusCode, [new()
            {
                Code = _errorCode,
                Description = uploadResult.Error.Message
            }]);
        }

        public async Task<Result<(string? publicId, string? absoluteUrl)>> UploadProductImageAsync(IFormFile file)
            => await UploadImageAsync(file, _productFolder);

        public async Task<Result<(string? publicId, string? absoluteUrl)>> UploadAvatarImageAsync(IFormFile file)
            => await UploadImageAsync(file, _userFolder);

        public async Task<Result<(string? publicId, string? absoluteUrl)>> UploadProductAvatarImageAsync(IFormFile file)
            => await UploadImageAsync(file, _productAvartarFolder);

        public async Task<Result<(string? publicId, string? absoluteUrl)>> UploadBlogImageAsync(IFormFile file)
            => await UploadImageAsync(file, _blogFolder);

        public async Task<Result<(string? publicId, string? absoluteUrl)>> UploadAvatarBlogImageAsync(IFormFile file)
            => await UploadImageAsync(file, _blogAvartarFolder);

        public async Task<Result<string>> RemoveImage(string publicId)
        {
            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image

            };

            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (result.Error != null)
            {
                return Result<string>.Failure((HttpStatusCode)result.StatusCode, [new()
                {
                    Code = _errorCode,
                    Description = result.Error.Message
                }]);
            }

            return Result<string>.Ok(result.Result);
        }
    }
}
