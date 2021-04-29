
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

        private string TypeFullName => this.GetType().FullName;


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
                _logger.LogError("{CloudinaryService}: {Method} File is empty - length equals : {fileLength}",TypeFullName, nameof(AddImageToCloud), file.Length);
                return new CloudImageUploadResult(false, new Error
                {
                    Errors = new []{"File not contain any byte information - Uploading terminated"}
                });
            }

            using var stream = file?.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file?.Name, stream)
            };
            

            var uploadResult = _cloudinary.Upload(uploadParams);
            if (uploadResult?.Url is null)
            {
                _logger.LogError("{CloudinaryService}: {Method} Uri from cloudinary is {uploadResultUrl}",
                    TypeFullName, nameof(AddImageToCloud), uploadResult?.Url);
                return new CloudImageUploadResult(false,new Error
                {
                    Errors = new [] {"Uri from cloudinary is null - Uploading terminated"}
                });
            }
            
            _logger.LogInformation("{CloudinaryService}: {Method} Successfully uploaded image : Url {Url}", 
                TypeFullName, nameof(AddImageToCloud), uploadResult.Url);
            return new CloudImageUploadResult(true, uploadResult.Url.ToString(), uploadResult.PublicId);
        }
    }

    public interface ICloudinaryService
    {
        CloudImageUploadResult AddImageToCloud(AddPhotoCommand command);
    }
}