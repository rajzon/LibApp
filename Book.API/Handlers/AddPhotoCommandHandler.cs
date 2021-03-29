using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Commands.V1;
using Book.API.Controllers.V1;
using Book.API.Domain;
using Book.API.Services;
using MediatR;

namespace Book.API.Handlers
{
    public class AddPhotoCommandHandler : IRequestHandler<AddPhotoCommand, AddPhotoCommandResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public AddPhotoCommandHandler(IBookRepository bookRepository,
            ICloudinaryService cloudinaryService,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }
        
        
        public async Task<AddPhotoCommandResult> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.FindByIdWithPhotoAsync(request.Id);

            if (book is null)
                return null;
            
            //Call cloudinary service
            var uploadResult = _cloudinaryService.AddImageToCloud(request);

            book.AddImage(uploadResult.Url, uploadResult.PublicId, request.IsMain);
            
            return await _bookRepository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0
                ? _mapper.Map<AddPhotoCommandResult>(book.Images.LastOrDefault())
                : null;

        }
    }
}