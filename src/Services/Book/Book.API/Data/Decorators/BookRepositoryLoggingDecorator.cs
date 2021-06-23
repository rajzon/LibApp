using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Book.API.Data.Repositories;
using Book.API.Domain;
using Book.API.Domain.Common;
using Book.API.Extensions;
using Microsoft.Extensions.Logging;

namespace Book.API.Data.Decorators
{
    internal class BookRepositoryLoggingDecorator : IBookRepository
    {
        private readonly ILogger<BookRepositoryLoggingDecorator> _logger;
        private readonly IBookRepository _bookRepository;
        private Type BookRepositoryType => _bookRepository.GetType();
        
        public IUnitOfWork UnitOfWork => _bookRepository.UnitOfWork;

        public BookRepositoryLoggingDecorator(IBookRepository bookRepository,
            ILogger<BookRepositoryLoggingDecorator> logger)
        {
            _logger = logger;
            _bookRepository = bookRepository;
        }
        
        public Domain.Book Add(Domain.Book book)
        {
            _logger.LogInfoCreateMethodStarted<Domain.Book>(BookRepositoryType, nameof(Add), new object[] {book});

            if (book is null)
                _logger.LogWarningCreateMethodNullData<Domain.Book>(BookRepositoryType, nameof(Add));
            
            var result = _bookRepository.Add(book);
            

            _logger.LogInfoCreateMethodEnded(BookRepositoryType, nameof(Add), result);
            return result;
        }

        public async Task<Domain.Book> FindByIdAsync(int id)
        {
            _logger.LogInfoMethodStarted<Domain.Book>(BookRepositoryType, nameof(FindByIdAsync),
                 new object[] {id});
            
            var result = await _bookRepository.FindByIdAsync(id);

            if (result is null)
                _logger.LogWarningNotFound<Domain.Book>(BookRepositoryType, nameof(FindByIdAsync), new object[] {id});
            
            _logger.LogInfoMethodEnded(BookRepositoryType, nameof(FindByIdAsync), result);

            return result;
        }

        public async Task<Domain.Book> FindByIdWithPhotoAsync(int id)
        {
            _logger.LogInfoMethodStarted<Domain.Book>(BookRepositoryType, nameof(FindByIdWithPhotoAsync), new object[] {id});


            var result = await _bookRepository.FindByIdWithPhotoAsync(id);
            
            if (result is null)
                _logger.LogWarningNotFound<Domain.Book>(BookRepositoryType, nameof(FindByIdWithPhotoAsync), new object[] {id});
            
            _logger.LogInfoMethodEnded(BookRepositoryType, nameof(FindByIdWithPhotoAsync), result);
            return result;
        }

        public async Task<bool> IsAllExistsAsync(Dictionary<int, string> booksIdsWithEans)
        {
            _logger.LogInfoMethodStarted<Domain.Book>(BookRepositoryType, nameof(FindByIdWithPhotoAsync), new object[] {booksIdsWithEans});


            var result = await _bookRepository.IsAllExistsAsync(booksIdsWithEans);
            
            
            _logger.LogInfoMethodEnded(BookRepositoryType, nameof(FindByIdWithPhotoAsync), result);
            return result;
        }

        public async Task<IEnumerable<Domain.Book>> GetAllAsync()
        {
            _logger.LogInfoMethodStarted<IEnumerable<Domain.Book>>(BookRepositoryType, nameof(GetAllAsync), methodParams: null);
            
            var result = await _bookRepository.GetAllAsync();

            if (! result.Any())
                _logger.LogWarningNotFound<IEnumerable<Domain.Book>>(BookRepositoryType, nameof(GetAllAsync), null);

            _logger.LogInfoMethodEnded(BookRepositoryType, nameof(GetAllAsync), result);
            return result;
        }

    }
}