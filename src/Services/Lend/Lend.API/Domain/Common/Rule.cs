namespace Lend.API.Domain.Common
{
    public abstract class Rule<T> : Entity
    {
        public string RuleName { get; protected set; }
        public string Description { get; protected set; }
        public T RuleValue { get; protected set; }
        public RuleValueType RuleValueType { get; protected set; }
        
        //Name of class that is responsible for process that rule
        public string StrategyType { get; protected set; }
        
    }
}