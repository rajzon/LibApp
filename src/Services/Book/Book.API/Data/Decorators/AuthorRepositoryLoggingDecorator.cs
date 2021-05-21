using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Book.API.Extensions;
using Microsoft.Extensions.Logging;

namespace Book.API.Data.Decorators
{
    internal class AuthorRepositoryLoggingDecorator : IAuthorRepository
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AuthorRepositoryLoggingDecorator> _logger;
        private Type AuthorRepositoryType => _authorRepository.GetType();
        
        public IUnitOfWork UnitOfWork => _authorRepository.UnitOfWork; 

        public AuthorRepositoryLoggingDecorator(IAuthorRepository authorRepository,
            ILogger<AuthorRepositoryLoggingDecorator> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }


        public Author Add(Author author)
        {
            _logger.LogInfoCreateMethodStarted<Author>(AuthorRepositoryType, nameof(Add), new object[] {author});

            if (author is null)
                _logger.LogWarningCreateMethodNullData<Author>(AuthorRepositoryType, nameof(Add));
            
            var result = _authorRepository.Add(author);

            _logger.LogInfoCreateMethodEnded(AuthorRepositoryType, nameof(Add), result);
            return result;
        }

        public async Task<Author> FindByIdAsync(int id)
        {
           _logger.LogInfoMethodStarted<Author>(AuthorRepositoryType, nameof(FindByIdAsync), new object[] {id}); 
            var result = await _authorRepository.FindByIdAsync(id);
            
            if (result is null)
                _logger.LogWarningNotFound<Author>(AuthorRepositoryType, nameof(FindByIdAsync), new object[] {id});


            _logger.LogInfoMethodEnded(AuthorRepositoryType, nameof(FindByIdAsync), result); 
            return result;
        }

        public async Task<Author> FindByNameAsync(AuthorName name)
        {
            _logger.LogInfoMethodStarted<Author>(AuthorRepositoryType, nameof(FindByNameAsync), new object[] {name});
            
            var result = await _authorRepository.FindByNameAsync(name);
            if (result is null)
                _logger.LogWarningNotFound<Author>(AuthorRepositoryType, nameof(FindByNameAsync), new object[] {name});
                
            
            _logger.LogInfoMethodEnded(AuthorRepositoryType, nameof(FindByNameAsync), result);
            return result;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            _logger.LogInfoMethodStarted<IEnumerable<Author>>(AuthorRepositoryType, nameof(GetAllAsync), null);
            
            var result = await _authorRepository.GetAllAsync();
            if (! result.Any())
                _logger.LogWarningNotFound<IEnumerable<Author>>(AuthorRepositoryType, nameof(GetAllAsync), null);
            
            _logger.LogInfoMethodEnded(AuthorRepositoryType, nameof(GetAllAsync), result);
            return result;
        }

        public async Task<IEnumerable<Author>> GetAllByIdAsync(int[] authorsIds)
        {
            _logger.LogInfoMethodStarted<IEnumerable<Author>>(AuthorRepositoryType, nameof(GetAllByIdAsync), new object[] {authorsIds});
            
            var result = await _authorRepository.GetAllByIdAsync(authorsIds);
            if (! result.Any())
                _logger.LogWarningNotFound<IEnumerable<Author>>(AuthorRepositoryType, nameof(GetAllByIdAsync), new object[] {authorsIds});
            
            _logger.LogInfoMethodEnded(AuthorRepositoryType, nameof(GetAllByIdAsync), result);
            return result;
        }
    }
}