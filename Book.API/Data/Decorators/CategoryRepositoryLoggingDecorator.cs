using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Book.API.Extensions;
using Microsoft.Extensions.Logging;

namespace Book.API.Data.Decorators
{
    public class CategoryRepositoryLoggingDecorator : ICategoryRepository
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryRepositoryLoggingDecorator> _logger;
        
        private Type CategoryRepositoryType => _categoryRepository.GetType();
        
        public IUnitOfWork UnitOfWork => _categoryRepository.UnitOfWork;

        public CategoryRepositoryLoggingDecorator(ICategoryRepository categoryRepository,
            ILogger<CategoryRepositoryLoggingDecorator> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }
        
        public Category Add(Category category)
        {
            _logger.LogInfoCreateMethodStarted<Category>(CategoryRepositoryType, nameof(Add), new object[] {category});
            
            if (category is null)
                _logger.LogWarningCreateMethodNullData<Category>(CategoryRepositoryType, nameof(Add));
            
            var result = _categoryRepository.Add(category);
            
            _logger.LogInfoCreateMethodEnded(CategoryRepositoryType, nameof(Add), result);
            return result;
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            _logger.LogInfoMethodStarted<Category>(CategoryRepositoryType, nameof(FindByIdAsync), new object[]{id});
            
            var result = await _categoryRepository.FindByIdAsync(id);
            
            if (result is null)
                _logger.LogWarningNotFound<Category>(CategoryRepositoryType, nameof(FindByIdAsync), new object[] {id});
            
            _logger.LogInfoMethodEnded(CategoryRepositoryType, nameof(FindByIdAsync), result);
            return result;
        }

        public async Task<IEnumerable<Category>> GetAllByNamesAsync(IList<string> names)
        {
            _logger.LogInfoMethodStarted<IEnumerable<Category>>(CategoryRepositoryType, nameof(GetAllByNamesAsync), new object[]{names});
            
            var result = await _categoryRepository.GetAllByNamesAsync(names);
            
            if (! result.Any())
                _logger.LogWarningNotFound<IEnumerable<Category>>(CategoryRepositoryType, nameof(GetAllByNamesAsync), new object[] {names});
            
            
            _logger.LogInfoMethodEnded(CategoryRepositoryType, nameof(GetAllByNamesAsync), result);
            return result;
        }

        public async Task<IEnumerable<Category>> GetAllByIdAsync(IList<int> categoriesIds)
        {
            _logger.LogInfoMethodStarted<IEnumerable<Category>>(CategoryRepositoryType, nameof(GetAllByIdAsync), new object[]{categoriesIds});

            
            var result = await _categoryRepository.GetAllByIdAsync(categoriesIds);
            
            if (! result.Any())
                _logger.LogWarningNotFound<IEnumerable<Category>>(CategoryRepositoryType, nameof(GetAllByIdAsync), new object[] {categoriesIds});

            _logger.LogInfoMethodEnded(CategoryRepositoryType, nameof(GetAllByIdAsync), result);
            return result;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            _logger.LogInfoMethodStarted<IEnumerable<Category>>(CategoryRepositoryType, nameof(GetAllAsync), null);
            
            var result = await _categoryRepository.GetAllAsync();
            
            if (! result.Any())
                _logger.LogWarningNotFound<IEnumerable<Category>>(CategoryRepositoryType, nameof(GetAllAsync), null);

            
            _logger.LogInfoMethodEnded(CategoryRepositoryType, nameof(GetAllAsync), result);
            return result;
        }
    }
}