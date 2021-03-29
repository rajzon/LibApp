using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Commands.V1;
using Book.API.Controllers.V1;
using Book.API.Domain;
using MediatR;

namespace Book.API.Handlers
{
    public class CreateBookManualCommandHandler : IRequestHandler<CreateBookManualCommand, CreateBookCommandResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateBookManualCommandHandler(IBookRepository bookRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        
        public async Task<CreateBookCommandResult> Handle(CreateBookManualCommand request, CancellationToken cancellationToken)
        {
            var isbn10 = new BookIsbn10(request.Isbn10);
            var isbn13 = new BookIsbn13(request.Isbn13);
            var categories = await _categoryRepository.GetAllByIdAsync(request.CategoriesIds);

            var book = new Domain.Book(request.Title,
                request.AuthorId,
                request.Description,
                isbn10,
                isbn13,
                request.LanguageId,
                request.PublisherId,
                request.PageCount,
                request.Visibility,
                request.PublishedDate);

            foreach (var category in categories)
            {
                book.AddCategory(category);
            }

            var result = _bookRepository.Add(book);
            await _bookRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateBookCommandResult>(result);

        }
    }
}