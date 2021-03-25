using System.Collections.Generic;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;

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
            return _bookDbContext.Publishers.Add(publisher).Entity;
        }

        public async Task<Publisher> FindByIdAsync(int id)
        {
            return await _bookDbContext.Publishers.FindAsync(id);
        }

        public async Task<Publisher> FindByNameAsync(string name)
        {
            return await _bookDbContext.Publishers.SingleOrDefaultAsync(p => p.Name.Equals(name));
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            return await _bookDbContext.Publishers.ToListAsync();
        }
    }
}