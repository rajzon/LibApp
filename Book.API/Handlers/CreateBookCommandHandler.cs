using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Commands.V1;
using Book.API.Commands.V1.Dtos;
using Book.API.Data.Repositories;
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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;


        public CreateBookCommandHandler(IBookRepository bookRepository,
            IMapper mapper,
            ILanguageRepository languageRepository,
            IAuthorRepository authorRepository,
            IPublisherRepository publisherRepository,
            ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _languageRepository = languageRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
            _categoryRepository = categoryRepository;
        }
        
        public async Task<CreateBookCommandResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var language = await GetOrCreateLanguageAsync(request, cancellationToken);

            var author = await GetOrCreateAuthorAsync(request, cancellationToken);

            var publisher = await GetOrCreatePublisherAsync(request, cancellationToken);

            var book = new Domain.Book(request.Title, author?.Id,
                request.Description, request.Isbn10,
                request.Isbn13, language?.Id,
                publisher?.Id, request.PageCount,
                request.Visibility, request.PublishedDate);

            if (request.CategoriesNames is not null && request.CategoriesNames.Any())
                await AddCategoriesToBookAsync(book, request.CategoriesNames.Distinct());

            var result = _bookRepository.Add(book);
            
            if (await _bookRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 1)
                return new CreateBookCommandResult(false, new[] {"Error occured during saving Book"});

            var bookResult = _mapper.Map<CommandBookDto>(result);
            return new CreateBookCommandResult(true, bookResult);
        }
        
        private async Task AddCategoriesToBookAsync(Domain.Book book,
            IEnumerable<string> categoriesNames)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var requestedCategoriesThatExists = categories.Where(c => categoriesNames.Contains(c.Name)).ToList();
            foreach (var categoryName in categoriesNames)
            {
                var category = requestedCategoriesThatExists.SingleOrDefault(c => c.Name.Equals(categoryName));
                if (category is not null)
                {
                    book.AddCategory(category);
                    continue;
                }
            
                book.AddCategory(new Category(categoryName));
            }
        }

        private async Task<Author> GetOrCreateAuthorAsync(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var author = request.Author is not null
                ? await _authorRepository.FindByNameAsync(new AuthorName(request.Author)) ??
                  _authorRepository.Add(new Author(new AuthorName(request.Author)))
                : null;

            await _authorRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return author;
        }

        private async Task<Language> GetOrCreateLanguageAsync(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var language = request.LanguageName is not null
                ? await _languageRepository.FindByNameAsync(request.LanguageName) ??
                  _languageRepository.Add(new Language(request.LanguageName))
                : null;


            await _languageRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return language;
        }
        
        private async Task<Publisher> GetOrCreatePublisherAsync(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var publisher = request.PublisherName is not null
                ? await _publisherRepository.FindByNameAsync(request.PublisherName) ??
                  _publisherRepository.Add(new Publisher(request.PublisherName))
                : null;

            await _publisherRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return publisher;
        }

        
    }
}