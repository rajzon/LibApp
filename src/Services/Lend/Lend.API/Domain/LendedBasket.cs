using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lend.API.Domain.Common;
using Lend.API.Domain.Strategies;

namespace Lend.API.Domain
{
    public class LendedBasket : Entity , IAggregateRoot
    {
        public string Email { get; private set; }
        public IEnumerable<LendedStock> Stocks { get; private set; }

        public LendedBasket(Basket basket, IEnumerable<IBaseStrategy> strategies)
        {
            if (basket is null)
                throw new ArgumentException("Basket cannot be null");
            if (HasAnyBooksInBasketAreAlreadyLendedByCustomer(basket, out List<int> conflictStocks))
                throw new ArgumentException(
                    $"Customer already have borrowed following stocks:{string.Join(",", conflictStocks)}");

            var match = IsBasketMatchAllStrategies(basket, strategies).Result;
            if (!match.Item1)
                throw new AggregateException($"Basket do not match strategy: {match.Item2.ErrorDescription}");
                
            
            Email = basket.Customer.Email.EmailAddress;
            Stocks = basket.StockWithBooks.Select(s => new LendedStock(s.StockId, s.ReturnDate));
        }

        protected LendedBasket()
        {
        }

        public async Task<(bool, StrategyError)> IsBasketMatchAllStrategies(Basket basket, IEnumerable<IBaseStrategy> strategies)
        {
            foreach (var strategy in strategies)
            {
                var match = await strategy.IsBasketMatchStrategy(basket);
                if (!match.Item1)
                    return match;
            }

            return (false, null);
        }
        

        public bool HasAnyBooksInBasketAreAlreadyLendedByCustomer(Basket basket, out List<int> conflictStocks)
        {
            if (basket.Customer.Email.EmailAddress != Email)
                throw new ArgumentException("Customer in basket is different than customer from lended basket");

            var stockFromBasket = basket.StockWithBooks.Select(s => s.StockId).ToList();
            var lendedStocks = Stocks.Select(s => s.StockId).ToList();

            conflictStocks = stockFromBasket.Intersect(lendedStocks).ToList();
            

            return conflictStocks.Any();
        }
    }

    public class LendedStock : Entity
    {
        public int StockId { get; private set; }
        public DateTime ReturnDate { get; private set; }

        public LendedStock(int stockId, DateTime returnDate)
        {
            StockId = stockId;
            ReturnDate = returnDate;
        }

        protected LendedStock()
        {
        }
    }
}