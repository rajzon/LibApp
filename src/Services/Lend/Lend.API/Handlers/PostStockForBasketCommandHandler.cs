using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using Lend.API.Controllers.V1;
using Lend.API.Domain;
using Lend.API.Domain.Repositories;
using Lend.API.Domain.Strategies;
using Lend.API.Extenstions;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Lend.API.Handlers
{
    public class PostStockForBasketCommandHandler : IRequestHandler<PostStockForBasketCommand, PostBasketCommandResult>
    {
        private readonly IRequestClient<GetStockWithBookInfo> _client;
        private readonly ILendedBasketRepository _lendedBasketRepository;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IStrategy<SimpleIntRule>> _intStrategies;
        private readonly IEnumerable<IStrategy<SimpleBooleanRule>> _booleanStrategies;

        public PostStockForBasketCommandHandler(IRequestClient<GetStockWithBookInfo> client, ILendedBasketRepository lendedBasketRepository,
            IMemoryCache cache, IMapper mapper, IEnumerable<IStrategy<SimpleIntRule>> intStrategies, IEnumerable<IStrategy<SimpleBooleanRule>> booleanStrategies)
        {
            _client = client;
            _lendedBasketRepository = lendedBasketRepository;
            _cache = cache;
            _mapper = mapper;
            _intStrategies = intStrategies;
            _booleanStrategies = booleanStrategies;
        }
        
        public async Task<PostBasketCommandResult> Handle(PostStockForBasketCommand request, CancellationToken cancellationToken)
        {
            //TODO check if BOOK is already lended to other customer, return Conflict!!!!!
            var warnings = new List<string>();
            var messageResponse = await _client.GetResponse<StockWithBookInfoResult>(new
            {
                StockId = request.StockId
            }, cancellationToken);

            var stockWithBookInfo =  messageResponse.Message.StockWithBookInfo;
            //TODO check if requested stock exists, return NotFound
            if (stockWithBookInfo is null)
                return new PostBasketCommandResult(false, new List<string>() {$"Stock {request.StockId} not found"});

            //TODO check if stockId is already lended to other customer, return Conflict
            var borrowedStocks  = await _lendedBasketRepository.GetAllByStockId(stockWithBookInfo.StockId);
            if (borrowedStocks.Any())
                return new PostBasketCommandResult(false,
                    new List<string>() {$"Stock {request.StockId} are already borrowed by someone"});
            
            
            //TODO FOR NEW Stocks: set default return date to max allowed by organization( call strategy/rule)
            Basket newBasket;
            if (!_cache.TryGetValue(request.EmployeeName, out Basket basket))
            {
                newBasket = new Basket(null,
                    new List<StockWithBooksBasket> {CreateStockWithBooksBasket(stockWithBookInfo)});
                
                var check = await CheckIfCustomerAndBasketMatchAllStrategies(newBasket);
                if (!check.Item1)
                    return new PostBasketCommandResult(false, new List<string>() {check.Item2.FirstOrDefault()});
                warnings.AddRange(check.Item3);
                
                _cache.Set(request.EmployeeName, newBasket);
            }
            //TODO do not allow same StockId to be placed in basket
            //TODO do not allow same BookEan to be placed in basket
            else
            {
                //TODO Workaround for problem with  setting default value for returnDate, after DeepCopy 
                var originalStocksReturnDate = basket.StockWithBooks.ToDictionary(k => k.StockId, v => v.ReturnDate);
                newBasket = basket.DeepCopy();
                foreach (var stock in newBasket.StockWithBooks)
                {
                    if (originalStocksReturnDate.TryGetValue(stock.StockId, out DateTime returnDate))
                        stock.ReturnDate = returnDate;
                }
                var newStock = CreateStockWithBooksBasket(stockWithBookInfo);
                if (newBasket.IsStockDuplicatedInBasket(newStock))
                    return new PostBasketCommandResult(false, new List<string>() {"Trying to add duplicate stock"});
                if (newBasket.IsBookEanDuplicatedInBasket(newStock))
                    return new PostBasketCommandResult(false, new List<string>() {"Trying to add duplicate book"});
                
                newBasket.AddNewStock(newStock);

                var check = await CheckIfCustomerAndBasketMatchAllStrategies(newBasket);
                if (!check.Item1)
                    return new PostBasketCommandResult(false, new List<string>() {check.Item2.FirstOrDefault()});
                warnings.AddRange(check.Item3);
                
                _cache.Remove(request.EmployeeName);
                _cache.Set(request.EmployeeName, newBasket);
            }
            
            
            
            //TODO wywołać IsBasketMatchStrategy(Basket basket) oraz Task<(bool, StrategyError)> IsCustomerMatchStrategy(CustomerBasket basket) !!!
            return new PostBasketCommandResult(true, new BasketResponseDto()
            {
                Customer = newBasket.Customer is not null ? _mapper.Map<CustomerBasketDto>(newBasket.Customer) : null,
                StockWithBooks = _mapper.Map<IEnumerable<StockWithBooksBasketDto>>(newBasket.StockWithBooks),
                BusinessWarnings = warnings,
            });
        }
        
        private async Task<(bool, List<string>, List<string>)> CheckIfCustomerAndBasketMatchAllStrategies(Basket basket)
        {
            var errors = new List<string>();
            var warnings = new List<string>();
            var result = true;
            
            foreach (var intStrategy in _intStrategies)
            {
                var match = await intStrategy.IsBasketMatchStrategy(basket);
                if (match.Item2 is not null)
                {
                    if (match.Item2.ErrorType == ErrorType.Error)
                        errors.Add(match.Item2.ErrorDescription);
                    else
                        warnings.Add(match.Item2.ErrorDescription);
                }
            }
            foreach (var booleanStrategy in _booleanStrategies)
            {
                var match = await booleanStrategy.IsBasketMatchStrategy(basket);
                if (match.Item2 is not null)
                {
                    if (match.Item2.ErrorType == ErrorType.Error)
                        errors.Add(match.Item2.ErrorDescription);
                    else
                        warnings.Add(match.Item2.ErrorDescription);
                }
            }

            if (errors.Any())
                result = false;
            return (result, errors, warnings);
        } 


        private StockWithBooksBasket CreateStockWithBooksBasket(StockWithBookInfoBusResponse stockWithBookInfo)
        {
            return new StockWithBooksBasket(stockWithBookInfo.StockId, stockWithBookInfo.Title, stockWithBookInfo.Ean13,
                stockWithBookInfo.Isbn10, stockWithBookInfo.Isbn13,
                stockWithBookInfo.PublishedDate, CreateCategoriesBasket(stockWithBookInfo.Categories),
                CreateAuthorsBasket(stockWithBookInfo.Authors),
                stockWithBookInfo.Publisher is not null? new PublisherBasket(stockWithBookInfo.Publisher.Id, stockWithBookInfo.Publisher.Name): null,
                stockWithBookInfo.Image is not null? new ImageBasket(stockWithBookInfo.Image.Url, stockWithBookInfo.Image.IsMain): null,
                _intStrategies);
        }

        private IEnumerable<CategoryBasket> CreateCategoriesBasket(IEnumerable<CategoryBusResponseDto> categoriesDto)
        {
            var result = new List<CategoryBasket>();
            foreach (var category in categoriesDto)
            {
                result.Add(new CategoryBasket(category.Id, category.Name));
            }

            return result;
        }
        
        private IEnumerable<AuthorBasket> CreateAuthorsBasket(IEnumerable<AuthorBusResponseDto> authorsDto)
        {
            var result = new List<AuthorBasket>();
            foreach (var author in authorsDto)
            {
                result.Add(new AuthorBasket(author.Id, author.FirstName, author.LastName, author.FullName));
            }

            return result;
        }
    }
}