
using System;
using System.Collections.Generic;
using Book.API.Commands.V1;
using Book.API.Controllers.V1;
using Book.API.Domain.Errors;
using Book.API.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Error = Book.API.Domain.Errors.Error;

namespace Book.API.Services
{

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;

        
        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            _cloudinarySettings = cloudinarySettings;
            var acc = new Account(_cloudinarySettings.Value.CloudName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }
        
        public CloudImageUploadResult AddImageToCloud(AddPhotoCommand command)
        {
            var file = command.File;
            if (file?.Length == 0)
                return new CloudImageUploadResult(new Error
                {
                    Errors = new []{"File not contain any byte information"}
                });

            using var stream = file?.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file?.Name, stream)
            };

            
            var uploadResult = _cloudinary.Upload(uploadParams);
            if (uploadResult?.Url is null)
                return new CloudImageUploadResult(new Error
                {
                    Errors = new[] {"Uri from cloudinary is null"}
                });
            
            return new CloudImageUploadResult(uploadResult.Url.ToString(), uploadResult.PublicId);
        }
    }

    public interface ICloudinaryService
    {
        CloudImageUploadResult AddImageToCloud(AddPhotoCommand command);
    }
}