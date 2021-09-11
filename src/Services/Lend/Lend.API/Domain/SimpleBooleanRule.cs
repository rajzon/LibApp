using System;
using Lend.API.Domain.Common;

namespace Lend.API.Domain
{
    public class SimpleBooleanRule : Rule<bool>, IAggregateRoot
    {
        public SimpleBooleanRule(string ruleName, string description, bool ruleValue, Type strategyType)
        {
            RuleName = ruleName;
            Description = description;
            RuleValue = ruleValue;
            RuleValueType = RuleValueType.boolean;
            StrategyType = strategyType.ToString();
        }

        protected SimpleBooleanRule()
        {
            
        }
    }
}