using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Commands.V1;
using Book.API.Commands.V1.Dtos;
using Book.API.Controllers.V1;
using Book.API.Domain;
using MediatR;

namespace Book.API.Handlers
{
    public class CreateBookManualCommandHandler : IRequestHandler<CreateBookManualCommand, CreateBookCommandResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public CreateBookManualCommandHandler(IBookRepository bookRepository,
            ICategoryRepository categoryRepository,
            IAuthorRepository authorRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        
        public async Task<CreateBookCommandResult> Handle(CreateBookManualCommand request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllByIdAsync(request.CategoriesIds);
            var authors = await _authorRepository.GetAllByIdAsync(request.AuthorsIds);

            var book = new Domain.Book(request.Title,
                request.Description, request.Isbn10,
                request.Isbn13, request.LanguageId,
                request.PublisherId, request.PageCount,
                request.Visibility, request.PublishedDate);

            foreach (var category in categories)
            {
                book.AddCategory(category);
            }

            foreach (var author in authors)
            {
                book.AddAuthor(author);
            }

            var result = _bookRepository.Add(book);
            if (await _bookRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 1)
                return new CreateBookCommandResult(false, new[] {"Error occured during saving Book"});

            var bookResult = _mapper.Map<CommandBookDto>(result);
            return new CreateBookCommandResult(true, bookResult);

        }
    }
}