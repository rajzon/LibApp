using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Book.API.Data.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly ILogger<PublisherRepository> _logger;
        private readonly BookDbContext _bookDbContext;

        private string TypeFullName => this.GetType().FullName;

        public IUnitOfWork UnitOfWork => _bookDbContext;

        public PublisherRepository(BookDbContext bookDbContext, ILogger<PublisherRepository> logger)
        {
            _logger = logger;
            _bookDbContext = bookDbContext;
        }

        
        public Publisher Add(Publisher publisher)
        {
            _logger.LogInformation("{PublisherRepository}: {PublisherRepositoryMethod} : Requesting adding new {Publisher} : Name {PublisherName}", 
                TypeFullName, nameof(Add), nameof(Publisher), publisher?.Name);
            
            if (publisher is null)
            {
                _logger.LogWarning("{PublisherRepository}: {PublisherRepositoryMethod} : {Publisher} to add is {PublisherValue}", 
                    TypeFullName, nameof(Add), nameof(Publisher), null);
            }
            
            var result = _bookDbContext.Publishers.Add(publisher).Entity;
            
            _logger.LogInformation("{PublisherRepository}: {PublisherRepositoryMethod} : Added new {Publisher} to be tracked by EF : Value {@PublisherObj} ", 
                TypeFullName, nameof(Add), nameof(Publisher), result);

            return result;
        }

        public async Task<Publisher> FindByIdAsync(int id)
        {
            var result = await _bookDbContext.Publishers.FindAsync(id);
            
            if (result is null)
                _logger.LogWarning("{PublisherRepository}: {PublisherRepositoryMethod} : Requested {Publisher} {PublisherId} not found", 
                    TypeFullName, nameof(FindByIdAsync), nameof(Publisher), id);
            
            _logger.LogInformation("{PublisherRepository}: {PublisherRepositoryMethod} : Request returned {Publisher} : {@PublisherObj}", 
                TypeFullName, nameof(FindByIdAsync), nameof(Publisher), result);
            
            return result;
        }

        public async Task<Publisher> FindByNameAsync(string name)
        {
            var result = await _bookDbContext.Publishers.SingleOrDefaultAsync(p => p.Name.Equals(name));
            
            if (result is null)
                _logger.LogWarning("{PublisherRepository}: {PublisherRepositoryMethod} : Requested {Publisher} {PublisherName} not found", 
                    TypeFullName, nameof(FindByNameAsync), nameof(Publisher), name);
            
            _logger.LogInformation("{PublisherRepository}: {PublisherRepositoryMethod} : Request returned {Publisher} : {@PublisherObj}", 
                TypeFullName, nameof(FindByNameAsync), nameof(Publisher), result);
            
            return result;
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            var result = await _bookDbContext.Publishers.ToListAsync();
            
            if (! result.Any())
                _logger.LogWarning("{PublisherRepository}: {PublisherRepositoryMethod} : Requested {Publishers} not found", 
                    GetType().FullName, nameof(GetAllAsync), typeof(IEnumerable<Publisher>));
            
            _logger.LogInformation("{PublisherRepository}: {PublisherRepositoryMethod} : Request returned {Publishers} Count {PublishersCount}", 
                GetType().FullName, nameof(GetAllAsync), typeof(IEnumerable<Publisher>), result.Count);
            
            return result;
        }
    }
}