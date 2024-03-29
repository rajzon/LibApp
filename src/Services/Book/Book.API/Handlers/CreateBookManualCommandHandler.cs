﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Commands.V1;
using Book.API.Commands.V1.Dtos;
using Book.API.Controllers.V1;
using Book.API.Domain;
using EventBus.Messages.Commands;
using EventBus.Messages.Common;
using MassTransit;
using MediatR;

namespace Book.API.Handlers
{
    public class CreateBookManualCommandHandler : IRequestHandler<CreateBookManualCommand, CreateBookCommandResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ILanguageRepository _languageRepository;
        private readonly IPublisherRepository _publisherRepository;

        public CreateBookManualCommandHandler(IBookRepository bookRepository,
            ICategoryRepository categoryRepository,
            IAuthorRepository authorRepository,
            IMapper mapper,
            ISendEndpointProvider sendEndpointProvider,
            ILanguageRepository languageRepository,
            IPublisherRepository publisherRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _sendEndpointProvider = sendEndpointProvider;
            _languageRepository = languageRepository;
            _publisherRepository = publisherRepository;
        }
        
        public async Task<CreateBookCommandResult> Handle(CreateBookManualCommand request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllByIdAsync(request.CategoriesIds);
            
            if (! categories.Any())
                return new CreateBookCommandResult(false, new[] {"Requested category/s not found"});

            var authors = await _authorRepository.GetAllByIdAsync(request.AuthorsIds);
            
            if (! authors.Any())
                return new CreateBookCommandResult(false, new[] {"Requested category/s not found"});

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
            
            var bookResultEvent = _mapper.Map<CreateBook>(result);
            bookResultEvent.Language = result.LanguageId > 0
                ? _mapper.Map<LanguageDto>(await _languageRepository.FindByIdAsync(result.LanguageId ?? 0))
                : null;
            bookResultEvent.Publisher = result.LanguageId > 0
                ? _mapper.Map<PublisherDto>(await _publisherRepository.FindByIdAsync(result.PublisherId ?? 0))
                : null;
            
            
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{EventBusConstants.CreateBookQueue}"));
            
            await endpoint.Send(bookResultEvent, cancellationToken);
            
            return new CreateBookCommandResult(true, bookResult);

        }
    }
}