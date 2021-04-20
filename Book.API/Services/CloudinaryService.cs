
using System;
using System.Collections.Generic;
using Book.API.Commands.V1;
using Book.API.Controllers.V1;
using Book.API.Domain.Errors;
using Book.API.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Error = Book.API.Domain.Errors.Error;

namespace Book.API.Services
{

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private readonly ILogger<CloudinaryService> _logger;


        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettings, ILogger<CloudinaryService> logger)
        {
            _cloudinarySettings = cloudinarySettings;
            _logger = logger;
            
            
            var acc = new Account(_cloudinarySettings.Value.CloudName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }
        
        public CloudImageUploadResult AddImageToCloud(AddPhotoCommand command)
        {
            var file = command.File;
            if (file?.Length == 0)
            {
                _logger.LogError("File is empty - length equals : {fileLength}", file.Length);
                return new CloudImageUploadResult(new Error
                {
                    Errors = new object[]{"File not contain any byte information - Uploading terminated"}
                });
            }

            using var stream = file?.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file?.Name, stream)
            };

            _logger.LogInformation("Requesting Cloudinary : Params {ImageFileParams}", uploadParams);
            var uploadResult = _cloudinary.Upload(uploadParams);
            if (uploadResult?.Url is null)
            {
                _logger.LogError("Uri from cloudinary is {uploadResultUrl}", uploadResult?.Url);
                return new CloudImageUploadResult(new Error
                {
                    Errors = new object[] {"Uri from cloudinary is null - Uploading terminated"}
                });
            }
            
            _logger.LogInformation("Successfully uploaded image : Url {Url}", uploadResult.Url);
            return new CloudImageUploadResult(uploadResult.Url.ToString(), uploadResult.PublicId);
        }
    }

    public interface ICloudinaryService
    {
        CloudImageUploadResult AddImageToCloud(AddPhotoCommand command);
    }
}