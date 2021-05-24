using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Commands.V1;
using Book.API.Commands.V1.Dtos;
using Book.API.Data.Repositories;
using Book.API.Domain;
using EventBus.Messages.Commands;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Book.API.Handlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, CreateBookCommandResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IPublisherRepository _publisherRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IMapper _mapper;


        public CreateBookCommandHandler(IBookRepository bookRepository,
            IMapper mapper,
            ILanguageRepository languageRepository,
            IAuthorRepository authorRepository,
            IPublisherRepository publisherRepository,
            ICategoryRepository categoryRepository, 
            ISendEndpointProvider sendEndpointProvider)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _languageRepository = languageRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
            _categoryRepository = categoryRepository;
            _sendEndpointProvider = sendEndpointProvider;
        }
        
        public async Task<CreateBookCommandResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var language = await GetOrCreateLanguageAsync(request, cancellationToken);
            
            var publisher = await GetOrCreatePublisherAsync(request, cancellationToken);

            var book = new Domain.Book(request.Title,
                request.Description, request.Isbn10,
                request.Isbn13, language?.Id,
                publisher?.Id, request.PageCount,
                request.Visibility, request.PublishedDate);

            if (request.CategoriesNames is not null && request.CategoriesNames.Any())
                await AddCategoriesToBookAsync(book, request.CategoriesNames.Distinct());
            
            
            if (request.AuthorsNames is not null && request.AuthorsNames.Any())
                await AddAuthorsToBookAsync(book, request.AuthorsNames);

            var result = _bookRepository.Add(book);
            
            if (await _bookRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 1)
                return new CreateBookCommandResult(false, new[] {"Error occured during saving Book"});

            var bookResult = _mapper.Map<CommandBookDto>(result);

            var bookResultEvent = _mapper.Map<CreateBook>(result);
            bookResultEvent.Language = language is not null ? _mapper.Map<LanguageDto>(language) : null;
            bookResultEvent.Publisher = publisher is not null ? _mapper.Map<PublisherDto>(publisher) : null;
            

            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{EventBusConstants.CreateBookQueue}"));
            
            await endpoint.Send(bookResultEvent, cancellationToken);

            return new CreateBookCommandResult(true, bookResult);
        }
        
        private async Task AddAuthorsToBookAsync(Domain.Book book, IEnumerable<string> authorsNames)
        {
            var authors = await _authorRepository.GetAllAsync();
            foreach (var name in authorsNames)
            {
                var authorThatExist = authors.SingleOrDefault(a => a.Name.FullName.Equals(name));
                if (authorThatExist is not null)
                {
                    book.AddAuthor(authorThatExist);
                    continue;
                }

                book.AddAuthor(new Author(new AuthorName(name)));
            }
        }
        
        private async Task AddCategoriesToBookAsync(Domain.Book book,
            IEnumerable<string> categoriesNames)
        {
            var categories = await _categoryRepository.GetAllAsync();
            foreach (var categoryName in categoriesNames)
            {
                var categoryThatExist = categories.SingleOrDefault(c => c.Name.Equals(categoryName));
                if (categoryThatExist is not null)
                {
                    book.AddCategory(categoryThatExist);
                    continue;
                }
            
                book.AddCategory(new Category(categoryName));
            }
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