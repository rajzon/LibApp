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
        private readonly ILogger<AuthorRepository> _logger;
        
        private string TypeFullName => this.GetType().FullName;
        
        public IUnitOfWork UnitOfWork => _bookDbContext;

        public AuthorRepository(BookDbContext bookDbContext, ILogger<AuthorRepository> logger)
        {
            _bookDbContext = bookDbContext;
            _logger = logger;
        }
        
        public Author Add(Author author)
        {
            _logger.LogInformation("{AuthorRepository}: {AuthorRepositoryMethod} : Requesting adding new {Author} : Name {AuthorFirstName} {AuthorLastName}", 
                TypeFullName, nameof(Add), nameof(Author), author?.Name?.FirstName, author?.Name?.LastName);
            
            if (author is null)
            {
                _logger.LogWarning("{AuthorRepository}: {AuthorRepositoryMethod} : {Author} to add is {AuthorValue}", 
                    TypeFullName, nameof(Add), nameof(Author), null);
            }
            
            var result = _bookDbContext.Authors.Add(author).Entity;
            
            _logger.LogInformation("{AuthorRepository}: {AuthorRepositoryMethod} : Added new {Author} with value {@AuthorObj} to be tracked by EF", 
                TypeFullName, nameof(Add), nameof(Author), result);
            
            return result;
        }

        public async Task<Author> FindByIdAsync(int id)
        {
            var result = await _bookDbContext.Authors.FindAsync(id);

            if (result is null)
                _logger.LogWarning("{AuthorRepository}: {AuthorRepositoryMethod} : Requested {Author} {AuthorId} not found", 
                    TypeFullName, nameof(FindByIdAsync), nameof(Author), id);
            
            
            _logger.LogInformation("{AuthorRepository}: {AuthorRepositoryMethod} : Request returned {Author} {@AuthorObj}", 
                TypeFullName, nameof(FindByIdAsync), nameof(Author), result);
            
            
            return result;
        }

        public async Task<Author> FindByNameAsync(AuthorName name)
        {
            var result = await _bookDbContext.Authors
                .SingleOrDefaultAsync(a => a.Name.FirstName.Equals(name.FirstName) &&
                                           a.Name.LastName.Equals(name.LastName));

            if (result is null)
                _logger.LogWarning("{AuthorRepository}: {AuthorRepositoryMethod}: Requested {Author} : {AuthorFirstName} {AuthorLastName} not found", 
                    TypeFullName, nameof(FindByNameAsync), nameof(Author), name.FirstName, name.LastName);
            
            
            _logger.LogInformation("{AuthorRepository}: {AuthorRepositoryMethod} : Request returned {Author} {@AuthorObj}", 
                TypeFullName, nameof(FindByNameAsync), nameof(Author), result);
            
            return result;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            var result = await _bookDbContext.Authors.ToListAsync();
            
            if (! result.Any())
                _logger.LogWarning("{AuthorRepository}: {AuthorRepositoryMethod} : Requested {Authors} not found", 
                    TypeFullName, nameof(GetAllAsync), typeof(IEnumerable<Author>));
            
            _logger.LogInformation("{AuthorRepository}: {AuthorRepositoryMethod} : Request returned {Authors} Count {AuthorsCount}", 
                TypeFullName, nameof(GetAllAsync), typeof(IEnumerable<Author>), result, result.Count);

            return result;
        }
    }
}