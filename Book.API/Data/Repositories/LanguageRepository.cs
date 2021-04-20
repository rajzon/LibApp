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
        private readonly ILogger<LanguageRepository> _logger;
        
        private string TypeFullName => this.GetType().FullName;
        
        public IUnitOfWork UnitOfWork => _bookContext;


        public LanguageRepository(BookDbContext bookContext, ILogger<LanguageRepository> logger)
        {
            _bookContext = bookContext;
            _logger = logger;
        }
        
        
        public Language Add(Language language)
        {
            _logger.LogInformation("{LanguageRepository}: {LanguageRepositoryMethod} : Requesting adding new {Language} : Name {LanguageName}", 
                TypeFullName, nameof(Add), nameof(Language), language?.Name);
            
            if (language is null)
            {
                _logger.LogWarning("{LanguageRepository}: {LanguageRepositoryMethod} : {Language} to add is {LanguageValue}", 
                    TypeFullName, nameof(Add), nameof(Language), null);
            }
            
            
            var result = _bookContext.Languages.Add(language).Entity;

            _logger.LogInformation("{LanguageRepository}: {LanguageRepositoryMethod} : Added new {Language} with value {@LanguageObj} to be tracked by EF", 
                TypeFullName, nameof(Add), nameof(Language), language);
            return result;
        }

        public async Task<Language> FindByIdAsync(int id)
        {
            var result = await _bookContext.Languages.FindAsync(id);
            
            if (result is null)
                _logger.LogWarning("{LanguageRepository}: {LanguageRepositoryMethod} : Requested {Language} {LanguageId} not found", 
                    TypeFullName, nameof(FindByIdAsync), nameof(Language), id);
            
            _logger.LogInformation("{LanguageRepository}: {LanguageRepositoryMethod} : Request returned {Language} : {@LanguageObj}", 
                TypeFullName, nameof(FindByIdAsync), nameof(Language), result);

            return result;
        }

        public async Task<Language> FindByNameAsync(string name)
        {
            var result = await _bookContext.Languages
                .SingleOrDefaultAsync(l => l.Name.Equals(name));

            if (result is null)
                _logger.LogWarning("{LanguageRepository}: {LanguageRepositoryMethod}: Requested {Language} {LanguageName} not found", 
                    TypeFullName, nameof(FindByNameAsync), nameof(Language), name);
            
            _logger.LogInformation("{LanguageRepository}: {LanguageRepositoryMethod} : Request returned {Language} : {@LanguageObj}", 
                TypeFullName, nameof(FindByNameAsync), nameof(Language), result);

            return result;
        }

        public async Task<IEnumerable<Language>> GetAllAsync()
        {
            var result = await _bookContext.Languages.ToListAsync();
            
            if (! result.Any())
                _logger.LogWarning("{LanguageRepository}: {LanguageRepositoryMethod} : Requested {Languages} not found", 
                    TypeFullName, nameof(GetAllAsync), typeof(IEnumerable<Language>));
            
            _logger.LogInformation("{LanguageRepository}: {LanguageRepositoryMethod} : Request returned {Languages} Count {LanguagesCount}", 
                TypeFullName, nameof(GetAllAsync), typeof(IEnumerable<Language>), result.Count);

            return result;
        }
    }
}