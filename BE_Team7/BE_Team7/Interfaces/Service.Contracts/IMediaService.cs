﻿using GarageManagementAPI.Shared.ResultModel;

namespace BE_Team7.Interfaces.Service.Contracts
{
    public interface IMediaService
    {
        public Task<Result<(string? publicId, string? absoluteUrl)>> UploadProductImageAsync(IFormFile file);
        public Task<Result<(string? publicId, string? absoluteUrl)>> UploadAvatarImageAsync(IFormFile file);
        public Task<Result<string>> RemoveImage(string publicId);
    }
}
