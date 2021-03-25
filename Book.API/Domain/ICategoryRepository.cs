using System.Collections.Generic;
using System.Threading.Tasks;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category Add(Category category);
        Task<Category> FindByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllByNamesAsync(IList<string> names);
        Task<IEnumerable<Category>> GetAllAsync();
    }
}