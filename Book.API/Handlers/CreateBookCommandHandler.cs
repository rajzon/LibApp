﻿using System.Collections.Generic;
using System.Linq;
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
            var language = request.LanguageName is not null
                ? await _languageRepository.FindByNameAsync(request.LanguageName) ??
                  _languageRepository.Add(new Language(request.LanguageName))
                : null;
            
            await _languageRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var author = request.Author is not null
                ? await _authorRepository.FindByNameAsync(new AuthorName(request.Author.FirstName,
                      request.Author.LastName)) ??
                  _authorRepository.Add(new Author(new AuthorName(request.Author.FirstName, request.Author.LastName)))
                : null;

            await _authorRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var publisher = request.PublisherName is not null
                ? await _publisherRepository.FindByNameAsync(request.PublisherName) ??
                  _publisherRepository.Add(new Publisher(request.PublisherName))
                : null;
            
            await _publisherRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            var book = new 
                Domain.Book(request.Title, 
                author?.Id,
                request.Description,
                request.Isbn10,
                request.Isbn13,
                language?.Id,
                publisher?.Id,
                request.PageCount,
                request.Visibility,
                request.PublishedDate);

            if (request.CategoriesNames is not null && request.CategoriesNames.Any())
                await AddCategoriesToBookAsync(book, request.CategoriesNames.Distinct());

            var result = _bookRepository.Add(book);
            await _bookRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<CreateBookCommandResult>(result);
        }

        private async Task AddCategoriesToBookAsync(Domain.Book book,
            IEnumerable<string> categoriesNames)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var requestedCategories = categories.Where(c => categoriesNames.Contains(c.Name)).ToList();
            foreach (var categoryName in categoriesNames)
            {
                var category = requestedCategories.SingleOrDefault(c => c.Name.Equals(categoryName));
                if (category is not null)
                {
                    book.AddCategory(category);
                    continue;
                }

                book.AddCategory(new Category(categoryName));
            }
        }
    }
}