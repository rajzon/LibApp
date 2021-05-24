using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Commands.V1;
using Book.API.Commands.V1.Dtos;
using Book.API.Controllers.V1;
using Book.API.Domain;
using Book.API.Services;
using EventBus.Messages.Commands;
using EventBus.Messages.Common;
using MassTransit;
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
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public AddPhotoCommandHandler(IBookRepository bookRepository,
            ICloudinaryService cloudinaryService,
            IMapper mapper, ILogger<AddPhotoCommandHandler> logger,
            ISendEndpointProvider sendEndpointProvider)
        {
            _bookRepository = bookRepository;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
            _logger = logger;
            _sendEndpointProvider = sendEndpointProvider;
        }
        
        public async Task<AddPhotoCommandResult> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.FindByIdWithPhotoAsync(request.Id);
            if (book is null)
                return new AddPhotoCommandResult(false, new []{$"Requested Book Id: {request.Id} not found"});
            
            //Call cloudinary service
            var uploadResult = _cloudinaryService.AddImageToCloud(request);
            if (! uploadResult.Succeeded)
            {
                _logger.LogError("Uploading Errors : {Errors}", uploadResult.Error.Errors);
                return new AddPhotoCommandResult(false, uploadResult.Error.Errors);
            }
            
            book.AddImage(uploadResult.Url, uploadResult.PublicId, request.IsMain);
            
            _logger.LogInformation("Saving {Image} : Url {Url} : IsMain: {IsMain} : to DB", nameof(Domain.Image),
                book.Images.Select(x => x.Url).LastOrDefault(), book.Images.Select(x => x.IsMain).LastOrDefault());
            
            
            if (await _bookRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 1)
            {
                _logger.LogError("Error occured during saving {Image} : Url {Url} : IsMain: {IsMain} : to DB", nameof(Domain.Image),
                    book.Images.Select(x => x.Url).LastOrDefault(), book.Images.Select(x => x.IsMain).LastOrDefault());
                 return new AddPhotoCommandResult(false, new []{$"Error occured during saving Image for Book Id: ${request.Id}"});
            }
            
            var photoResult = _mapper.Map<CommandPhotoDto>(book.Images.LastOrDefault()); ;
            var addImageEvent = new AddImageToBook
            {
                BookId = book.Id,
                Image = _mapper.Map<ImageDto>(book.Images.LastOrDefault())
            };

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{EventBusConstants.AddImageToBookQueue}"));
            await endpoint.Send(addImageEvent, cancellationToken);
            
            return new AddPhotoCommandResult(true, photoResult);

        }
    }
}