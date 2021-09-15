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
        public List<LendedStock> Stocks { get; private set; }

        public LendedBasket(Basket basket, IEnumerable<IStrategy<SimpleIntRule>> intStrategies,
            IEnumerable<IStrategy<SimpleBooleanRule>> booleanStrategies)
        {
            if (basket is null)
                throw new ArgumentException("Basket cannot be null");

            var match = IsBasketMatchAllStrategies(basket, intStrategies, booleanStrategies).Result;
            if (!match.Item1)
                throw new AggregateException($"Basket do not match strategy: {match.Item2.ErrorDescription}");
                
            
            Email = basket.Customer.Email.EmailAddress;
            Stocks = basket.StockWithBooks.Select(s => new LendedStock(s.StockId, s.Ean13, s.ReturnDate)).ToList();
        }

        protected LendedBasket()
        {
        }

        public async Task<(bool, StrategyError)> IsBasketMatchAllStrategies(Basket basket, IEnumerable<IStrategy<SimpleIntRule>> intStrategies,
            IEnumerable<IStrategy<SimpleBooleanRule>> booleanStrategies)
        {
            foreach (var strategy in intStrategies)
            {
                var match = await strategy.IsBasketMatchStrategy(basket);
                if (!match.Item1)
                    return match;
            }

            foreach (var booleanStrategy in booleanStrategies)
            {
                var match = await booleanStrategy.IsBasketMatchStrategy(basket);
                if (!match.Item1)
                    return match;
            }

            return (true, null);
        }
        

        public bool HasAnyStocksInBasketAreAlreadyLendedByCustomer(Basket basket, out List<int> conflictStocks)
        {
            if (basket.Customer.Email.EmailAddress != Email)
                throw new ArgumentException("Customer in basket is different than customer from lended basket");

            var stockFromBasket = basket.StockWithBooks.Select(s => s.StockId).ToList();
            var lendedStocks = Stocks.Select(s => s.StockId).ToList();

            conflictStocks = stockFromBasket.Intersect(lendedStocks).ToList();
            

            return conflictStocks.Any();
        }
        
        public bool HasAnyBooksInBasketAreAlreadyLendedByCustomer(Basket basket, out List<string> conflictEans)
        {
            if (basket.Customer.Email.EmailAddress != Email)
                throw new ArgumentException("Customer in basket is different than customer from lended basket");

            var eansFromBasket = basket.StockWithBooks.Select(s => s.Ean13).ToList();
            var lendedEans = Stocks.Select(s => s.BookEan).ToList();

            conflictEans = eansFromBasket.Intersect(lendedEans).ToList();
            

            return conflictEans.Any();
        }
    }

    public class LendedStock : Entity
    {
        public int StockId { get; private set; }
        public string BookEan { get; private set; }
        public DateTime ReturnDate { get; private set; }

        public LendedStock(int stockId, string bookEan, DateTime returnDate)
        {
            StockId = stockId;
            ReturnDate = returnDate;
            BookEan = bookEan;
        }

        protected LendedStock()
        {
        }
    }
}