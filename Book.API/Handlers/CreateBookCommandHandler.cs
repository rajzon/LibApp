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
        private readonly ILanguageRepository _languageRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly IMapper _mapper;


        public CreateBookCommandHandler(IBookRepository bookRepository,
            IMapper mapper,
            ILanguageRepository languageRepository,
            IAuthorRepository authorRepository,
            IPublisherRepository publisherRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _languageRepository = languageRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
        }
        
        public async Task<CreateBookCommandResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            //TODO implement EAN13 generation
            var ean13 = new BookEan13("1234567890123");
            var isbn10 = new BookIsbn10(request.Isbn10);
            var isbn13 = new BookIsbn13(request.Isbn13);
            
            //TODO only for test purposes
            var language = await _languageRepository.FindByNameAsync("English") ??
                           _languageRepository.Add(new Language("English"));
            
            await _languageRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            var author = await _authorRepository.FindByNameAsync(new AuthorName("Andrzej", "Sapkowski")) ??
                         _authorRepository.Add(new Author(new AuthorName("Andrzej", "Sapkowski")));

            await _authorRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var publisher = await _publisherRepository.FindByNameAsync("Nowa Era") ??
                            _publisherRepository.Add(new Publisher("Nowa Era"));
            
            await _publisherRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            var book = new 
                Domain.Book(request.Title,
                ean13,
                author.Id,
                request.Description,
                isbn10,
                isbn13,
                language.Id,
                publisher.Id);

            book.AddCategory(new Category("Fantasy"));

            var result = _bookRepository.Add(book);
            await _bookRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<CreateBookCommandResult>(result);
        }
    }
}