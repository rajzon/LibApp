using System;
using System.Linq;
using System.Threading.Tasks;
using Lend.API.Domain;
using Lend.API.Domain.Common;
using Lend.API.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lend.API.Data.Repositories
{
    public class SimpleBooleanRuleRepository : ISimpleBooleanRuleRepository
    {
        private LendDbContext _lendDbContext;
        
        
        public IUnitOfWork UnitOfWork => _lendDbContext;

        public SimpleBooleanRuleRepository(LendDbContext lendDbContext)
        {
            _lendDbContext = lendDbContext;
        }

        public async Task<SimpleBooleanRule> GetByStrategyTypeAsync(Type strategyType)
        {
            return await _lendDbContext.SimpleBooleanRules.FirstOrDefaultAsync(r => r.StrategyType == strategyType.ToString());
        }
    }
}