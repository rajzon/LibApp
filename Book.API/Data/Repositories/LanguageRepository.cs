using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Data.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly BookDbContext _bookContext;
        public IUnitOfWork UnitOfWork => _bookContext;


        public LanguageRepository(BookDbContext bookContext)
        {
            _bookContext = bookContext;
        }
        
        
        public Language Add(Language language)
        {
            return _bookContext.Languages.Add(language).Entity;
        }

        public async Task<Language> FindByIdAsync(int id)
        {
            return await _bookContext.Languages.FindAsync(id);
        }

        public async Task<Language> FindByNameAsync(string name)
        {
            return await _bookContext.Languages
                .SingleOrDefaultAsync(l => l.Name.Equals(name));
        }

        public async Task<IEnumerable<Language>> GetAllAsync()
        {
            return await _bookContext.Languages.ToListAsync();
        }
    }
}