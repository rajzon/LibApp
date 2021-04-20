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
        private readonly ILogger<BookRepository> _logger;

        private string TypeFullName => this.GetType().FullName;
        public IUnitOfWork UnitOfWork => _bookContext;

        public BookRepository(BookDbContext bookContext, ILogger<BookRepository> logger)
        {
            _bookContext = bookContext;
            _logger = logger;
        }
        public Domain.Book Add(Domain.Book book)
        {
            
            _logger.LogInformation("{BookRepository}: {BookRepositoryMethod} : Requesting adding new {Book} : Value {@BookObj}", 
                TypeFullName, nameof(Add), nameof(Domain.Book), book);
            
            
            if (book is null)
            {
                _logger.LogWarning("{BookRepository}: {BookRepositoryMethod} : {Book} to add is {BookValue}", 
                    TypeFullName, nameof(Add), nameof(Domain.Book), null);
            }
            
            
            var result = _bookContext.Books.Add(book).Entity;
            
            _logger.LogInformation("{BookRepository}: {BookRepositoryMethod} : Added new {Book} with Value {@BookObj} to be tracked by EF", 
                TypeFullName, nameof(Add), nameof(Domain.Book), result);
            
            return result;
        }

        //TODO: Consider some sort of builder for building Book with related tables instead of explicitly including tables
        public async Task<Domain.Book> FindByIdAsync(int id)
        {
            var result = await _bookContext.Books
                .Include(b => b.Categories)
                .SingleOrDefaultAsync(b => b.Id.Equals(id));
            
            if (result is null)
                _logger.LogWarning("{BookRepository}: {BookRepositoryMethod}: Requested {Book} {BookId} not found", 
                    TypeFullName, nameof(FindByIdAsync), nameof(Domain.Book), id);
            
            
            _logger.LogInformation("{BookRepository}: {BookRepositoryMethod} : Request returned {Book} : {@BookObj}", 
                TypeFullName, nameof(FindByIdAsync), nameof(Domain.Book), result);
            
            return result;
        }

        public async Task<Domain.Book> FindByIdWithPhotoAsync(int id)
        {
            var result = await _bookContext.Books
                .Include(i => i.Images)
                .SingleOrDefaultAsync(b => b.Id.Equals(id));
            
            if (result is null)
                _logger.LogWarning("{BookRepository}: {BookRepositoryMethod}: Requested {Book} {BookId} not found", 
                    TypeFullName, nameof(FindByIdWithPhotoAsync), nameof(Domain.Book), id);
            
            _logger.LogInformation("{BookRepository}: {BookRepositoryMethod} : Request returned {Book} : {@BookObj}", 
                TypeFullName, nameof(FindByIdWithPhotoAsync), nameof(Domain.Book), result);
            
            return result;
        }

        //TODO: Consider some sort of builder for building Book with related tables
        public async Task<IEnumerable<Domain.Book>> GetAllAsync()
        {
            var result = await _bookContext.Books
                .Include(b => b.Categories)
                .ToListAsync();
            
            if (! result.Any())
                _logger.LogWarning("{BookRepository}: {BookRepositoryMethod} : Requested {Books} not found", 
                    TypeFullName, nameof(GetAllAsync), typeof(IEnumerable<Domain.Book>));
            
            _logger.LogInformation("{BookRepository}: {BookRepositoryMethod} : Request returned {Books} Count {Count}", 
                TypeFullName, nameof(GetAllAsync), typeof(IEnumerable<Domain.Book>), result.Count);
            
            return result;
        }
    }
}