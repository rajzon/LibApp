using System;
using Lend.API.Domain.Common;

namespace Lend.API.Domain
{
    public class SimpleIntRule : Rule<int>, IAggregateRoot 
    {

        public SimpleIntRule(string ruleName, string description, int ruleValue, Type strategyType)
        {
            RuleName = ruleName;
            Description = description;
            RuleValue = ruleValue;
            RuleValueType = RuleValueType.intiger;
            StrategyType = strategyType.ToString();
        }

        protected SimpleIntRule()
        {
            
        }
    }
}