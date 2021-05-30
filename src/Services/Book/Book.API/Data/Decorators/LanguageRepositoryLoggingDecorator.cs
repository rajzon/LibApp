using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Domain.Common;
using Book.API.Extensions;
using Microsoft.Extensions.Logging;

namespace Book.API.Data.Decorators
{
    public class LanguageRepositoryLoggingDecorator : ILanguageRepository
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ILogger<LanguageRepositoryLoggingDecorator> _logger;

        private Type LanguageRepositoryType => _languageRepository.GetType();
        public IUnitOfWork UnitOfWork => _languageRepository.UnitOfWork;

        public LanguageRepositoryLoggingDecorator(ILanguageRepository languageRepository,
            ILogger<LanguageRepositoryLoggingDecorator> logger)
        {
            _languageRepository = languageRepository;
            _logger = logger;
        }
        
        public  Language Add(Language language)
        {
            _logger.LogInfoCreateMethodStarted<Language>(LanguageRepositoryType, nameof(Add), new object[] {language});
            
            if (language is null)
                _logger.LogWarningCreateMethodNullData<Language>(LanguageRepositoryType, nameof(Add));
            
            var result =  _languageRepository.Add(language);
            
            
            _logger.LogInfoCreateMethodEnded(LanguageRepositoryType, nameof(Add), result);
            return  result;
        }

        public async Task<Language> FindByIdAsync(int id)
        {
            _logger.LogInfoCreateMethodStarted<Language>(LanguageRepositoryType, nameof(FindByIdAsync), new object[] {id});

            
            var result = await _languageRepository.FindByIdAsync(id);
            
            if (result is null)
                _logger.LogWarningNotFound<Language>(LanguageRepositoryType, nameof(FindByIdAsync), new object[] {id});
                
            
            _logger.LogInfoMethodEnded(LanguageRepositoryType, nameof(FindByIdAsync), result);
            return  result;
        }
        
        public async Task<Language> FindByNameAsync(string name)
        {
            _logger.LogInfoMethodStarted<Language>(LanguageRepositoryType, nameof(FindByNameAsync), new object[] {name});
            
            var result = await _languageRepository.FindByNameAsync(name);
            
            if (result is null)
                _logger.LogWarningNotFound<Language>(LanguageRepositoryType, nameof(FindByNameAsync), new object[] {name});                
            
            _logger.LogInfoMethodEnded(LanguageRepositoryType, nameof(FindByNameAsync), result);
            return result;
        }
        
        public async Task<IEnumerable<Language>> GetAllAsync()
        {
            _logger.LogInfoMethodStarted<IEnumerable<Language>>(LanguageRepositoryType, nameof(GetAllAsync), null);
            
            var result = await _languageRepository.GetAllAsync();
            
            if (! result.Any())
                _logger.LogWarningNotFound<IEnumerable<Language>>(LanguageRepositoryType, nameof(GetAllAsync), null);
                
            
            _logger.LogInfoMethodEnded(LanguageRepositoryType, nameof(GetAllAsync), result);
            return  result;
        }
    }
}