using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

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
            var result = _bookContext.Books.Add(book).Entity;

            return result;
        }

        //TODO: Consider some sort of builder for building Book with related tables instead of explicitly including tables
        public async Task<Domain.Book> FindByIdAsync(int id)
        {
            var result = await _bookContext.Books
                .Include(b => b.Categories)
                .SingleOrDefaultAsync(b => b.Id.Equals(id));
            
            return result;
        }

        public async Task<Domain.Book> FindByIdWithPhotoAsync(int id)
        {
            var result = await _bookContext.Books
                .Include(i => i.Images)
                .SingleOrDefaultAsync(b => b.Id.Equals(id));
            
            return result;
        }

        public async Task<bool> IsAllExistsAsync(Dictionary<int, string> booksIdsWithEans)
        {
            var result = await _bookContext.Books.AllAsync(b => 
                booksIdsWithEans.ContainsKey(b.Id) &&
                booksIdsWithEans.ContainsValue(b.Ean13.Code));

            return result;
        }

        public async Task<IEnumerable<Domain.Book>> GetAllByIds(List<int> booksIds)
        {
            var result = await _bookContext.Books
                .Include(b => b.Categories)
                .Include(b => b.Authors).Where(b => booksIds.Contains(b.Id))
                .Include(b => b.Images)
                .ToListAsync();

            return result;
        }

        //TODO: Consider some sort of builder for building Book with related tables
        public async Task<IEnumerable<Domain.Book>> GetAllAsync()
        {
            var result = await _bookContext.Books
                .Include(b => b.Categories)
                .Include(b => b.Authors)
                .ToListAsync();
            
            return result;
        }
    }
}