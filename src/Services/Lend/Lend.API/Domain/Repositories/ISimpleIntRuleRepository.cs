using System;
using System.Threading.Tasks;
using Lend.API.Domain.Common;

namespace Lend.API.Domain.Repositories
{
    public interface ISimpleIntRuleRepository : IRepository<SimpleIntRule>
    {
        Task<SimpleIntRule> GetByStrategyTypeAsync(Type strategyType);
    }
}