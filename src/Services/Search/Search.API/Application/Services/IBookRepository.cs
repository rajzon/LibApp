using System.Threading.Tasks;
using Nest;
using Search.API.Commands;
using Search.API.Domain;

namespace Search.API.Application.Services
{
    public interface IBookRepository : Common.IRepository<Book>
    {
        Task<ISearchResponse<Book>> SearchAsync(SearchBookCommand command);
    }
}