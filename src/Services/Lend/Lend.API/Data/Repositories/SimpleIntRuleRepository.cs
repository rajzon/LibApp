using System;
using System.Threading.Tasks;
using Lend.API.Domain;
using Lend.API.Domain.Common;
using Lend.API.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lend.API.Data.Repositories
{
    public class SimpleIntRuleRepository :  ISimpleIntRuleRepository
    {
        private LendDbContext _lendDbContext;
        
        
        public IUnitOfWork UnitOfWork => _lendDbContext;

        public SimpleIntRuleRepository(LendDbContext lendDbContext)
        {
            _lendDbContext = lendDbContext;
        }

        public async Task<SimpleIntRule> GetByStrategyTypeAsync(Type strategyType)
        {
            return await _lendDbContext.SimpleIntRules.FirstOrDefaultAsync(r => r.StrategyType == strategyType.ToString());
        }
    }
}