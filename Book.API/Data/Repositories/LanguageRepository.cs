using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            var result = _bookContext.Languages.Add(language).Entity;

            return result;
        }

        public async Task<Language> FindByIdAsync(int id)
        {
            var result = await _bookContext.Languages.FindAsync(id);
            
            return result;
        }

        public async Task<Language> FindByNameAsync(string name)
        {
            var result = await _bookContext.Languages
                .SingleOrDefaultAsync(l => l.Name.Equals(name));
            
            return result;
        }

        public async Task<IEnumerable<Language>> GetAllAsync()
        {
            var result = await _bookContext.Languages.ToListAsync();

            return result;
        }
    }
}