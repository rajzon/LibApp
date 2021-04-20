using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Commands.V1;
using Book.API.Controllers.V1;
using Book.API.Domain;
using Book.API.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Book.API.Handlers
{
    public class AddPhotoCommandHandler : IRequestHandler<AddPhotoCommand, AddPhotoCommandResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPhotoCommandHandler> _logger;

        public AddPhotoCommandHandler(IBookRepository bookRepository,
            ICloudinaryService cloudinaryService,
            IMapper mapper, ILogger<AddPhotoCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<AddPhotoCommandResult> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.FindByIdWithPhotoAsync(request.Id);

            if (book is null)
                return null;

            //Call cloudinary service
            _logger.LogInformation("Requesting {CloudinaryServiceRequest} : Args {Args}", 
                _cloudinaryService.GetType().GetMethod(nameof(_cloudinaryService.AddImageToCloud))?.Name, request);
            
            var uploadResult = _cloudinaryService.AddImageToCloud(request);
            if (uploadResult.Error is not null && uploadResult.Error.Errors.Any())
            {
                _logger.LogError("Uploading Errors : {Errors}", uploadResult.Error.Errors);
                return null;
            }
            
            book.AddImage(uploadResult.Url, uploadResult.PublicId, request.IsMain);
            
            _logger.LogInformation("Saving {Image} : Url {Url} : IsMain: {IsMain} : to DB", nameof(Domain.Image),
                book.Images.Select(x => x.Url).LastOrDefault(), book.Images.Select(x => x.IsMain).LastOrDefault());
            
            
            if (await _bookRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 1)
            {
                _logger.LogError("Error occured during saving {Image} : Url {Url} : IsMain: {IsMain} : to DB", nameof(Domain.Image),
                    book.Images.Select(x => x.Url).LastOrDefault(), book.Images.Select(x => x.IsMain).LastOrDefault());
                return null;
            }
            
            return _mapper.Map<AddPhotoCommandResult>(book.Images.LastOrDefault());
            
        }
    }
}