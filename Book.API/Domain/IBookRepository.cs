using System.Threading.Tasks;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public interface IBookRepository : IRepository<Book>
    {
        Book Add(Book book);
        Task<Book> FindByIdAsync(int id);
    }
}