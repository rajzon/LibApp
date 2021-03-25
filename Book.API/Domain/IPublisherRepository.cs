using System.Collections.Generic;
using System.Threading.Tasks;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public interface IPublisherRepository : IRepository<Publisher>
    {
        Publisher Add(Publisher publisher);
        Task<Publisher> FindByIdAsync(int id);
        Task<Publisher> FindByNameAsync(string name);
        Task<IEnumerable<Publisher>> GetAllAsync();
    }
}