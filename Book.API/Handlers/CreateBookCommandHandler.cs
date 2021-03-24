using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Commands.V1;
using Book.API.Domain;
using MediatR;

namespace Book.API.Handlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, CreateBookCommandResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;


        public CreateBookCommandHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        
        public async Task<CreateBookCommandResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var ean13 = new BookEan13(request.Ean13);
            var isbn10 = new BookIsbn10(request.Isbn10);
            var isbn13 = new BookIsbn13(request.Isbn13);
            var book = new Domain.Book(request.Title, ean13, request.Description, isbn10, isbn13);

            var result = _bookRepository.Add(book);
            await _bookRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<CreateBookCommandResult>(result);
        }
    }
}