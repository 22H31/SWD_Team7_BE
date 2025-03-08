using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using static BE_Team7.Shared.Result;
using BE_Team7.Dtos;
using BE_Team7.Shared.Contanst;
using BE_Team7.Shared;
using BE_Team7.Interfaces.Service.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

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

        private async Task<Results<(string? publicId, string? absoluteUrl)>> UploadImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                return Results<(string? publicId, string? absoluteUrl)>.BadRequest([RequestErrors.GetFileNotFoundErrors()]);
            }

            if (file.Length > _maxFileSize)
            {
                return Results<(string? publicId, string? absoluteUrl)>.BadRequest([RequestErrors.GetFileTooLargeErrors()]);
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !_permittedExtensions.Contains(extension))
            {
                return Results<(string? publicId, string? absoluteUrl)>.BadRequest([RequestErrors.GetFileExtensionInvalidErrors()]);
            }

            if (!_permittedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                return Results<(string? publicId, string? absoluteUrl)>.BadRequest([RequestErrors.GetFileTypeInvalidErrors()]);
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
                return Results<(string? publicId, string? absoluteUrl)>.Ok((publicId: uploadResult.PublicId, absoluteUrl: uploadResult.SecureUrl.AbsoluteUri));
            }

            return Results<(string? publicId, string? absoluteUrl)>.Ok((publicId: uploadResult.PublicId, absoluteUrl: uploadResult.SecureUrl.AbsoluteUri));
        }

        public async Task<Results<(string? publicId, string? absoluteUrl)>> UploadUserImageAsync(IFormFile file)
            => await UploadImageAsync(file, _userFolder);
        public async Task<Results<(string? publicId, string? absoluteUrl)>> UploadProductImageAsync(IFormFile file)
            => await UploadImageAsync(file, _productFolder);
        public async Task<Results<(string? publicId, string? absoluteUrl)>> UploadServiceImageAsync(IFormFile file)
            => await UploadImageAsync(file, _productFolder);
        public async Task<Results<(string? publicId, string? absoluteUrl)>> UploadBrandImageAsync(IFormFile file)
            => await UploadImageAsync(file, _productFolder);
        public async Task<Results<string>> RemoveImage(string publicId)
        {

            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image

            };

            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (result.Error != null)
            {
                return Results<string>.Ok(result.Result); ;
            }

            return Results<string>.Ok(result.Result);
        }

        Results<string> IMediaService.RemoveImage(string publicId)
        {
            throw new NotImplementedException();
        }

        Task<Results<(string? publicId, string? absoluteUrl)>> IMediaService.UploadUserImageAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }

        Task<Results<(string? publicId, string? absoluteUrl)>> IMediaService.UploadProductImageAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }

        Task<Results<(string? publicId, string? absoluteUrl)>> IMediaService.UploadServiceImageAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }

        Task<Results<(string? publicId, string? absoluteUrl)>> IMediaService.UploadBrandImageAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }

        //Results<string> IMediaService.RemoveImage(string publicId)
        //{
        //    return publicId;
        //}
    }
}
