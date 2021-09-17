using System;
using System.Linq;
using System.Threading.Tasks;
using Lend.API.Domain.Repositories;

namespace Lend.API.Domain.Strategies
{
    public class MaxDaysForLendBookStrategy : IStrategy<SimpleIntRule>
    {
        private readonly ISimpleIntRuleRepository _repository;

        public MaxDaysForLendBookStrategy(ISimpleIntRuleRepository repository)
        {
            _repository = repository;
        }
        public async Task<(bool, StrategyError)> IsBasketMatchStrategy(Basket basket)
        {
            var rule = await GetRuleInfo();
            if (basket.StockWithBooks.Any(s => s.ReturnDate > DateTime.UtcNow.AddDays(rule.RuleValue)))
                return (false, new StrategyError()
                {
                    ErrorType = ErrorType.Error,
                    ErrorDescription = $"Return Date cannot be set to more than {rule.RuleValue} days."
                });

            return (true, null);
        }

        public Task<(bool, StrategyError)> IsCustomerMatchStrategy(CustomerBasket basket)
        {
            throw new NotImplementedException();
        }

        public async Task<SimpleIntRule> GetRuleInfo()
        {
            var rule = await _repository.GetByStrategyTypeAsync(typeof(MaxDaysForLendBookStrategy));
            return rule;
        }
    }

    public interface IBaseStrategy
    {
        public Task<(bool, StrategyError)> IsBasketMatchStrategy(Basket basket);
        public Task<(bool, StrategyError)> IsCustomerMatchStrategy(CustomerBasket basket);
    }

    public interface IStrategy<T> : IBaseStrategy
        where  T: class
    {
        public Task<T> GetRuleInfo();
    }

    public class StrategyError
    {
        public string ErrorDescription { get; init; }
        public ErrorType ErrorType { get; init; }
    }

    public enum ErrorType
    {
        Warning,
        Error
    }


    public interface ICustomerStrategy<T> : IStrategy<T>
        where T : class
    {
          
    }
}