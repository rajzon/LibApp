using System.Collections.Generic;
using System.Threading.Tasks;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public interface ILanguageRepository : IRepository<Language>
    {
        Language Add(Language language);

        Task<Language> FindByIdAsync(int id);
        
        Task<Language> FindByNameAsync(string name);

        Task<IEnumerable<Language>> GetAllAsync();
    }
}