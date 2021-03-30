using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookDbContext _bookDbContext;

        public IUnitOfWork UnitOfWork => _bookDbContext;

        public CategoryRepository(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }

        public Category Add(Category category)
        {
            return _bookDbContext.Categories.Add(category).Entity;
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            return await _bookDbContext.Categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllByNamesAsync(IList<string> names)
        {
            return names is not null && names.Any()
                ? await _bookDbContext.Categories.Where(c => names.Distinct().Contains(c.Name)).ToListAsync()
                : Enumerable.Empty<Category>();
        }

        public async Task<IEnumerable<Category>> GetAllByIdAsync(IList<int> categoriesIds)
        {
            return categoriesIds is not null && categoriesIds.Any()
                ? await _bookDbContext.Categories.Where(c => categoriesIds.Distinct().Contains(c.Id)).ToListAsync()
                : Enumerable.Empty<Category>();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _bookDbContext.Categories.ToListAsync();
        }
    }
}