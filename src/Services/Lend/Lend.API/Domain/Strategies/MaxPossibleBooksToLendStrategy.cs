using System;
using System.Linq;
using System.Threading.Tasks;
using Lend.API.Domain.Repositories;

namespace Lend.API.Domain.Strategies
{
    public class MaxPossibleBooksToLendStrategy : IStrategy<SimpleIntRule>
    {
        private readonly ISimpleIntRuleRepository _repository;
        private readonly ILendedBasketRepository _lendedBasketRepository;

        public MaxPossibleBooksToLendStrategy(ISimpleIntRuleRepository repository, ILendedBasketRepository lendedBasketRepository)
        {
            _repository = repository;
            _lendedBasketRepository = lendedBasketRepository;
            // if (simpleIntRule.StrategyType != typeof(MaxPossibleBooksToLendStrategy).ToString())
            //     throw new ArgumentException("Passed Rule that is not intended for that strategy");
            //
            // _rule = simpleIntRule;
        }

        
        /// <summary>
        /// Checks: basket, rule and customer already borrowed books
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public async Task<(bool, StrategyError)> IsBasketMatchStrategy(Basket basket)
        {
            var rule =  await GetRuleInfo();
            if (rule is null)
                return (true, null);

            var basketStocksCount = basket.GetNumberOfStocks(); 
            if (basketStocksCount > rule.RuleValue)
                return (false,
                    new StrategyError()
                        {ErrorType = ErrorType.Error, ErrorDescription = $"Basket already have {basketStocksCount} books, max possible is {rule.RuleValue}"});

            
            var customerBorrowedBaskets = await _lendedBasketRepository.GetAllByCustomerEmail(basket.Customer.Email.EmailAddress);
            int lendedStocksCount = 0;
            foreach (var customerBorrowedBasket in customerBorrowedBaskets)
            {
                lendedStocksCount += customerBorrowedBasket.Stocks.Count();
            }

            if (lendedStocksCount >= rule.RuleValue)
                return (false,
                    new StrategyError()
                    {
                        ErrorType = ErrorType.Error,
                        ErrorDescription = $"Customer already have {lendedStocksCount} borrowed books, max possible is {rule.RuleValue}"
                    });

            var sumOfBasketStockAndBorrowed = basketStocksCount + lendedStocksCount;
            if (sumOfBasketStockAndBorrowed > rule.RuleValue)
                return (false,
                    new StrategyError()
                    {
                        ErrorType = ErrorType.Error,
                        ErrorDescription =
                            $"Sum of basket stocks and all already borrowed books by Customer, max possible is {rule.RuleValue}"
                    });

            
            
            return (true,
                new StrategyError()
                {
                    ErrorType = ErrorType.Warning,
                    ErrorDescription = $"Customer:({basket.Customer.Email.EmailAddress}) have some books borrowed"
                });
            
        }

        /// <summary>
        /// Checks: basket, rule and customer already borrowed books
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<(bool, StrategyError)> IsCustomerMatchStrategy(CustomerBasket customer)
        {
            var rule = await GetRuleInfo();
            if (rule is null)
                return (true, null);

            var customerBorrowedBooks = await _lendedBasketRepository.GetAllByCustomerEmail(customer.Email.EmailAddress);
            int stocksCount = 0;
            foreach (var customerBorrowedBook in customerBorrowedBooks)
            {
                stocksCount += customerBorrowedBook.Stocks.Count();
            }

            if (stocksCount <= rule.RuleValue)
                return (true,
                    new StrategyError()
                    {
                        ErrorType = ErrorType.Warning,
                        ErrorDescription = $"Customer:({customer.Email.EmailAddress}) have some books borrowed"
                    });

            return (false,
                new StrategyError()
                {
                    ErrorType = ErrorType.Error,
                    ErrorDescription = $"Customer already have {stocksCount} borrowed books, max possible is {rule.RuleValue}"
                });
        }


        public async Task<SimpleIntRule> GetRuleInfo()
        {
            var rule = await _repository.GetByStrategyTypeAsync(typeof(MaxPossibleBooksToLendStrategy));
            return rule;
        }
    }
    
    // public class CanCustomerBorrowBookIfAlreadyHaveBorrowedBooksStrategy : ICustomerStrategy<SimpleBooleanRule>
    // {
    //     private readonly ISimpleBooleanRuleRepository _repository;
    //
    //     public CanCustomerBorrowBookIfAlreadyHaveBorrowedBooksStrategy(ISimpleBooleanRuleRepository repository)
    //     {
    //         _repository = repository;
    //     }
    //     
    //     public async Task<(bool, StrategyError)> IsBasketMatchStrategy(Basket basket)
    //     {
    //         var rule = await _repository.GetByStrategyTypeAsync(typeof(CanCustomerBorrowBookIfAlreadyHaveBorrowedBooksStrategy));
    //
    //         if (!rule.RuleValue)
    //             return (true, null);
    //         
    //         
    //     }
    //
    //     public Task<SimpleBooleanRule> GetRuleInfo()
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}