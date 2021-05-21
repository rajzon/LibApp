using System.Collections.Generic;
using System.Threading.Tasks;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Author Add(Author author);
        Task<Author> FindByIdAsync(int id);
        Task<Author> FindByNameAsync(AuthorName name);
        Task<IEnumerable<Author>> GetAllAsync();
        Task<IEnumerable<Author>> GetAllByIdAsync(int[] authorsIds);
    }
}