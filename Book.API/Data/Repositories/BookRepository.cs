using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _bookContext;

        public IUnitOfWork UnitOfWork => _bookContext;

        public BookRepository(BookDbContext bookContext)
        {
            _bookContext = bookContext;
        }
        public Domain.Book Add(Domain.Book book)
        {
            return _bookContext.Books.Add(book).Entity;
        }

        //TODO: Consider some sort of builder for building Book with related tables instead of explicitly including tables
        public async Task<Domain.Book> FindByIdAsync(int id)
        {
            return await _bookContext.Books
                .Include(b => b.Categories)
                .SingleOrDefaultAsync(b => b.Id.Equals(id));
        }

        //TODO: Consider some sort of builder for building Book with related tables
        public async Task<IEnumerable<Domain.Book>> GetAllAsync()
        {
            return await _bookContext.Books
                .Include(b => b.Categories)
                .ToListAsync();
        }
    }
}