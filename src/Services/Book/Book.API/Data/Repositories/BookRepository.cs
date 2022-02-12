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
            var booksIds = booksIdsWithEans.Keys.ToList();
            var books = await _bookContext.Books
                .Include(b => b.Ean13)
                .Where(b => booksIds.Contains(b.Id)).ToListAsync();

            if (books.Count != booksIdsWithEans.Count)
                return false;

            foreach (var book in books)
            {
                if (booksIdsWithEans.TryGetValue(book.Id, out string ean))
                {
                    if (book.Ean13.Code != ean)
                        return false;
                }
                else
                    return false;
            }
            return true;
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