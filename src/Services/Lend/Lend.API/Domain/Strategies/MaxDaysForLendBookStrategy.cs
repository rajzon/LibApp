using System;

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

        public bool IsBasketMatchStrategy(Basket basket)
        {
            throw new NotImplementedException();
        }
    }

    public interface IStrategy<T> 
        where  T: class
    {
        public bool IsBasketMatchStrategy(Basket basket);
    }
}