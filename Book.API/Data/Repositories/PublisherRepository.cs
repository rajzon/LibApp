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
        private readonly BookDbContext _bookDbContext;
        
        public IUnitOfWork UnitOfWork => _bookDbContext;

        public PublisherRepository(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        
        public Publisher Add(Publisher publisher)
        {
            var result = _bookDbContext.Publishers.Add(publisher).Entity;

            return result;
        }

        public async Task<Publisher> FindByIdAsync(int id)
        {
            var result = await _bookDbContext.Publishers.FindAsync(id);
            
            return result;
        }

        public async Task<Publisher> FindByNameAsync(string name)
        {
            var result = await _bookDbContext.Publishers.SingleOrDefaultAsync(p => p.Name.Equals(name));

            return result;
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            var result = await _bookDbContext.Publishers.ToListAsync();
            
            
            return result;
        }
    }
}