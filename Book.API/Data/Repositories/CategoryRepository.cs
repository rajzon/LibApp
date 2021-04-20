using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Book.API.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookDbContext _bookDbContext;
        private readonly ILogger<Category> _logger;
        
        private string TypeFullName => this.GetType().FullName;

        public IUnitOfWork UnitOfWork => _bookDbContext;

        public CategoryRepository(BookDbContext bookDbContext, ILogger<Category> logger)
        {
            _bookDbContext = bookDbContext;
            _logger = logger;
        }

        public Category Add(Category category)
        {
            _logger.LogInformation("{CategoryRepository}: {CategoryRepositoryMethod} : Requesting adding new {Category} : Name {CategoryName}", 
                TypeFullName, nameof(Add), nameof(Category), category?.Name);
            
            if (category is null)
            {
                _logger.LogWarning("{CategoryRepository}: {CategoryRepositoryMethod} : {Category} to add is {CategoryValue}", 
                    TypeFullName, nameof(Add), nameof(Category), null);
            }
            
            
            var result = _bookDbContext.Categories.Add(category).Entity;
            
            _logger.LogInformation("{CategoryRepository}: {CategoryRepositoryMethod} : Added new {Category} with value {@CategoryObj} to be tracked by EF", 
                TypeFullName, nameof(Add), nameof(Category), result);
            
            return result;
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            var result = await _bookDbContext.Categories.FindAsync(id);
            
            if (result is null)
                _logger.LogWarning("{CategoryRepository}: {CategoryRepositoryMethod} : Requested {Category} {CategoryId} not found", 
                    TypeFullName, nameof(FindByIdAsync), nameof(Category), id);
            
            _logger.LogInformation("{CategoryRepository}: {CategoryRepositoryMethod} : Request returned {Category} : {@CategoryObj}", 
                TypeFullName, nameof(FindByIdAsync), nameof(Category), result);
            
            return result;
        }

        public async Task<IEnumerable<Category>> GetAllByNamesAsync(IList<string> names)
        {
            var result = names is not null && names.Any()
                ? await _bookDbContext.Categories.Where(c => names.Distinct().Contains(c.Name)).ToListAsync()
                : Enumerable.Empty<Category>().ToList();
            
            if (! result.Any())
                _logger.LogWarning("{CategoryRepository}: {CategoryRepositoryMethod} : Requested {Categories} Count {CategoriesCount} with names {CategoriesNames}  not found", 
                    TypeFullName, nameof(GetAllByNamesAsync), typeof(IEnumerable<Category>), names?.Count, names?.ToArray());
         
            
            _logger.LogInformation("{CategoryRepository}: {CategoryRepositoryMethod} : Request returned {Categories} Count {CategoriesCount} with names {CategoriesNames}", 
                TypeFullName, nameof(GetAllByNamesAsync), typeof(IEnumerable<Category>), result.Count, result.Select(x => x.Name).ToArray());
            
            return result;
        }

        public async Task<IEnumerable<Category>> GetAllByIdAsync(IList<int> categoriesIds)
        {
            var result = categoriesIds is not null && categoriesIds.Any()
                ? await _bookDbContext.Categories.Where(c => categoriesIds.Distinct().Contains(c.Id)).ToListAsync()
                : Enumerable.Empty<Category>().ToList();
            
            if (! result.Any())
                _logger.LogWarning("{CategoryRepository}: {CategoryRepositoryMethod} : Requested {Categories} Count {CategoriesCount} with ids {CategoriesIds}  not found", 
                    TypeFullName, nameof(GetAllByIdAsync), typeof(IEnumerable<Category>), categoriesIds?.Count, categoriesIds?.ToArray());
            
            _logger.LogInformation("{CategoryRepository}: {CategoryRepositoryMethod} : Request returned {Categories} Count {CategoriesCount} with ids {CategoriesIds}", 
                TypeFullName, nameof(GetAllByIdAsync), typeof(IEnumerable<Category>), result.Count, result.Select(x => x.Id).ToArray());

            return result;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var result = await _bookDbContext.Categories.ToListAsync();
            
            if (! result.Any())
                _logger.LogWarning("{CategoryRepository}: {CategoryRepositoryMethod} : Requested {Categories} not found", 
                    TypeFullName, nameof(GetAllAsync), typeof(IEnumerable<Category>));
            
            _logger.LogInformation("{CategoryRepository}: {CategoryRepositoryMethod} : Request returned {Categories} Count {CategoriesCount}", 
                TypeFullName, nameof(GetAllAsync), typeof(IEnumerable<Category>), result.Count);
            
            return result;
        }
    }
}