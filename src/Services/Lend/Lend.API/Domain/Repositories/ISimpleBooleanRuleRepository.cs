using System;
using System.Threading.Tasks;
using Lend.API.Domain.Common;

namespace Lend.API.Domain.Repositories
{
    public interface ISimpleBooleanRuleRepository : IRepository<SimpleBooleanRule>
    {
        Task<SimpleBooleanRule> GetByStrategyTypeAsync(Type strategyType);
    }
}