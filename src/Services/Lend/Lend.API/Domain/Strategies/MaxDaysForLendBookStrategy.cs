using System;
using System.Threading.Tasks;

namespace Lend.API.Domain.Strategies
{
    public class MaxDaysForLendBookStrategy : IStrategy<SimpleIntRule>
    {
        private SimpleIntRule _rule;

        public MaxDaysForLendBookStrategy(SimpleIntRule simpleIntRule)
        {
            if (simpleIntRule.StrategyType != typeof(MaxDaysForLendBookStrategy).ToString())
                throw new ArgumentException("Passed Rule that is not intended for that strategy");
            
            _rule = simpleIntRule;
        }
        public Task<(bool, StrategyError)> IsBasketMatchStrategy(Basket basket)
        {
            throw new NotImplementedException();
        }

        public Task<(bool, StrategyError)> IsCustomerMatchStrategy(CustomerBasket basket)
        {
            throw new NotImplementedException();
        }

        public Task<SimpleIntRule> GetRuleInfo()
        {
            throw new NotImplementedException();
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