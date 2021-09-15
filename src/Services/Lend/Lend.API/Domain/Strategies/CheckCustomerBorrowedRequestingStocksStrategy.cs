using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lend.API.Domain.Repositories;

namespace Lend.API.Domain.Strategies
{
    public class CheckCustomerBorrowedRequestingStocksStrategy : IStrategy<SimpleBooleanRule>
    {
        private readonly ISimpleBooleanRuleRepository _repository;
        private readonly ILendedBasketRepository _lendedBasketRepository;

        public CheckCustomerBorrowedRequestingStocksStrategy(ISimpleBooleanRuleRepository repository, ILendedBasketRepository lendedBasketRepository)
        {
            _repository = repository;
            _lendedBasketRepository = lendedBasketRepository;
        }
        
        public async Task<SimpleBooleanRule> GetRuleInfo()
        {
            var rule = await _repository.GetByStrategyTypeAsync(typeof(CheckCustomerDebtorStrategy));
            return rule;
        }

        public async Task<(bool, StrategyError)> IsBasketMatchStrategy(Basket basket)
        {
            //TODO check if user have any stocks already lended and they are placed in basket, then append error collection [domain rule]
            var rule = await GetRuleInfo();
            if (rule is null || !rule.RuleValue)
                return (true, null);
            if (basket.Customer is null)
                return (true, null);

            var customerLendedBaskets = await 
                _lendedBasketRepository.GetAllByCustomerEmail(basket.Customer.Email.EmailAddress);

            var conflictedStock = new StringBuilder();
            foreach (var customerBasket in customerLendedBaskets)
            {
                if (customerBasket.HasAnyStocksInBasketAreAlreadyLendedByCustomer(basket, out List<int> conflictStocks))
                {
                    conflictedStock.Append(string.Join(',', conflictStocks));
                }
            }

            if (conflictedStock.Length > 0)
                return (false, new StrategyError()
                {
                    ErrorType = ErrorType.Error,
                    ErrorDescription = $"Customer already have borrowed following stocks:{conflictedStock}"
                });

            return (true, null);
        }

        public Task<(bool, StrategyError)> IsCustomerMatchStrategy(CustomerBasket basket)
        {
            throw new System.NotImplementedException();
        }
    }
}