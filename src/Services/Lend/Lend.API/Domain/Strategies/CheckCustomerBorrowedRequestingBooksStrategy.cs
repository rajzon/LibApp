using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lend.API.Domain.Repositories;

namespace Lend.API.Domain.Strategies
{
    public class CheckCustomerBorrowedRequestingBooksStrategy : IStrategy<SimpleBooleanRule>
    {
        private readonly ISimpleBooleanRuleRepository _repository;
        private readonly ILendedBasketRepository _lendedBasketRepository;

        public CheckCustomerBorrowedRequestingBooksStrategy(ISimpleBooleanRuleRepository repository, ILendedBasketRepository lendedBasketRepository)
        {
            _repository = repository;
            _lendedBasketRepository = lendedBasketRepository;
        }
        
        public async Task<SimpleBooleanRule> GetRuleInfo()
        {
            return await _repository.GetByStrategyTypeAsync(typeof(CheckCustomerBorrowedRequestingBooksStrategy));
        }

        public async Task<(bool, StrategyError)> IsBasketMatchStrategy(Basket basket)
        {
            //TODO check if user have any books already lended and they are placed in basket, then append error collection [domain rule]
            var rule = await GetRuleInfo();
            if (rule is null || !rule.RuleValue || basket.Customer is null)
                return (true, null);
            
            var customerLendedBaskets = await 
                _lendedBasketRepository.GetAllByCustomerEmail(basket.Customer.Email.EmailAddress);

            var conflictedEans = new StringBuilder();
            foreach (var customerBasket in customerLendedBaskets)
            {
                if (customerBasket.HasAnyBooksInBasketAreAlreadyLendedByCustomer(basket, out List<string> conflictEans))
                {
                    conflictedEans.Append(string.Join(',', conflictEans));
                }
            }

            if (conflictedEans.Length > 0)
                return (false, new StrategyError()
                {
                    ErrorType = ErrorType.Error,
                    ErrorDescription = $"Customer already have borrowed following books:{conflictedEans}"
                });

            return (true, null);
        }

        public Task<(bool, StrategyError)> IsCustomerMatchStrategy(CustomerBasket basket)
        {
            throw new System.NotImplementedException();
        }
    }
}