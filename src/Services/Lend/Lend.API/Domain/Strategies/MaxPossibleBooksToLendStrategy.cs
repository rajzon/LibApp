using System;

namespace Lend.API.Domain.Strategies
{
    public class MaxPossibleBooksToLendStrategy : IStrategy<SimpleIntRule>
    {
        private SimpleIntRule _rule;

        public MaxPossibleBooksToLendStrategy(SimpleIntRule simpleIntRule)
        {
            if (simpleIntRule.StrategyType != typeof(MaxPossibleBooksToLendStrategy).ToString())
                throw new ArgumentException("Passed Rule that is not intended for that strategy");
            
            _rule = simpleIntRule;
        }
        
        public bool IsBasketMatchStrategy(Basket basket)
        {
            throw new System.NotImplementedException();
        }
    }
}