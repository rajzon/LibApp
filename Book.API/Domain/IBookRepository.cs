using System.Threading.Tasks;

namespace Book.API.Domain
{
    public interface IBookRepository
    {
        Book Add(Book book);
    }
}