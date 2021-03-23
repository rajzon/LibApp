using System.Threading.Tasks;
using Book.API.Data;
using Book.API.Domain;

namespace Book.API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _bookContext;

        public BookRepository(BookDbContext bookContext)
        {
            _bookContext = bookContext;
        }
        public Domain.Book Add(Domain.Book book)
        {
            return _bookContext.Books.Add(book).Entity;
        }
    }
}