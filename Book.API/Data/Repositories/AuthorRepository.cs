using System.Collections.Generic;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private BookDbContext _bookDbContext;
        public IUnitOfWork UnitOfWork => _bookDbContext;

        public AuthorRepository(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }
        
        public Author Add(Author author)
        {
            return _bookDbContext.Authors.Add(author).Entity;
        }

        public async Task<Author> FindByIdAsync(int id)
        {
            return await _bookDbContext.Authors.FindAsync(id);
        }

        public async Task<Author> FindByNameAsync(AuthorName name)
        {
            return await _bookDbContext.Authors
                .SingleOrDefaultAsync(a => a.Name.FirstName.Equals(name.FirstName) &&
                                           a.Name.LastName.Equals(name.LastName));
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _bookDbContext.Authors.ToListAsync();
        }
    }
}