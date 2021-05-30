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
    public class PublisherRepositoryLoggingDecorator : IPublisherRepository
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly ILogger<PublisherRepositoryLoggingDecorator> _logger;

        private Type PublisherRepositoryType => _publisherRepository.GetType();
        public IUnitOfWork UnitOfWork => _publisherRepository.UnitOfWork;
        
        
        public PublisherRepositoryLoggingDecorator(IPublisherRepository publisherRepository,
            ILogger<PublisherRepositoryLoggingDecorator> logger)
        {
            _publisherRepository = publisherRepository;
            _logger = logger;
        }
        
        public Publisher Add(Publisher publisher)
        {
            _logger.LogInfoCreateMethodStarted<Publisher>(PublisherRepositoryType, nameof(Add), new object[] {publisher});
            
            if (publisher is null)
                _logger.LogWarningCreateMethodNullData<Publisher>(PublisherRepositoryType, nameof(Add));
            
            var result = _publisherRepository.Add(publisher);
            
            _logger.LogInfoCreateMethodEnded(PublisherRepositoryType, nameof(Add), result);
            return result;
        }

        public async Task<Publisher> FindByIdAsync(int id)
        {
            _logger.LogInfoMethodStarted<Publisher>(PublisherRepositoryType, nameof(FindByIdAsync), new object[] {id});
            
            var result = await _publisherRepository.FindByIdAsync(id);
            
            if (result is null)
                _logger.LogWarningNotFound<Publisher>(PublisherRepositoryType, nameof(FindByIdAsync), new object[] {id});
            
            
            _logger.LogInfoMethodEnded(PublisherRepositoryType, nameof(FindByIdAsync), result);
            return result;
        }

        public async Task<Publisher> FindByNameAsync(string name)
        {
            _logger.LogInfoMethodStarted<Publisher>(PublisherRepositoryType, nameof(FindByNameAsync), new object[] {name});
            
            var result = await _publisherRepository.FindByNameAsync(name);
            
            if (result is null)
                _logger.LogWarningNotFound<Publisher>(PublisherRepositoryType, nameof(FindByNameAsync), new object[] {name});
            
            
            _logger.LogInfoMethodEnded(PublisherRepositoryType, nameof(FindByNameAsync), result);
            return result;
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            _logger.LogInfoMethodStarted<IEnumerable<Publisher>>(PublisherRepositoryType, nameof(GetAllAsync), null);
            
            var result = await _publisherRepository.GetAllAsync();
            
            if (! result.Any())
                _logger.LogWarningNotFound<IEnumerable<Publisher>>(PublisherRepositoryType, nameof(GetAllAsync), null);
            
            
            _logger.LogInfoMethodEnded(PublisherRepositoryType, nameof(GetAllAsync), result);
            return result;
        }
    }
}