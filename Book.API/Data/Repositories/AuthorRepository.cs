using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Book.API.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private BookDbContext _bookDbContext;
        
        
        public IUnitOfWork UnitOfWork => _bookDbContext;

        public AuthorRepository(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }
        
        public Author Add(Author author)
        {
            var result = _bookDbContext.Authors.Add(author).Entity;
            
            return result;
        }

        public async Task<Author> FindByIdAsync(int id)
        {
            var result = await _bookDbContext.Authors.FindAsync(id);
            
            
            return result;
        }

        public async Task<Author> FindByNameAsync(AuthorName name)
        {
            var result = await _bookDbContext.Authors
                .SingleOrDefaultAsync(a => a.Name.FullName.Equals(name.FullName));
            
            
            return result;
        }
        
        public async Task<IEnumerable<Author>> GetAllByIdAsync(int[] authorsIds)
        {
            var result = authorsIds is not null && authorsIds.Any()
                ? await _bookDbContext.Authors.Where(a => authorsIds.Contains(a.Id)).ToListAsync()
                : Enumerable.Empty<Author>().ToList();
            
            return result;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            var result = await _bookDbContext.Authors.ToListAsync();
            

            return result;
        }

        
    }
}