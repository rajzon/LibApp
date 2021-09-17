using System;
using System.Linq;
using System.Threading.Tasks;
using Lend.API.Domain.Repositories;

namespace Lend.API.Domain.Strategies
{
    public class CheckCustomerDebtorStrategy : IStrategy<SimpleBooleanRule>
    {
        private readonly ISimpleBooleanRuleRepository _repository;
        private readonly ILendedBasketRepository _lendedBasketRepository;

        public CheckCustomerDebtorStrategy(ISimpleBooleanRuleRepository repository, ILendedBasketRepository lendedBasketRepository)
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
            var rule = await GetRuleInfo();
            if (rule is null || !rule.RuleValue || basket.Customer is null)
                return (true, null);
            
            var customerBorrowedBaskets =
                await _lendedBasketRepository.GetAllByCustomerEmail(basket.Customer.Email.EmailAddress);
            foreach (var customerBorrowedBasket in customerBorrowedBaskets)
            {
                if (customerBorrowedBasket.Stocks.Any(s => s.ReturnDate < DateTime.UtcNow))
                {
                    return rule.RuleValue
                        ? (false, new StrategyError()
                        {
                            ErrorType = ErrorType.Error,
                            ErrorDescription = "Customer is debtor"
                        })
                        : (true, new StrategyError()
                        {
                            ErrorType = ErrorType.Warning,
                            ErrorDescription = "Customer is debtor"
                        });
                }
            }

            return (true, null);
        }

        public Task<(bool, StrategyError)> IsCustomerMatchStrategy(CustomerBasket basket)
        {
            throw new System.NotImplementedException();
        }
    }
}