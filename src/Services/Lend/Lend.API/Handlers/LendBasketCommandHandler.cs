using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Messages.Commands;
using EventBus.Messages.Common;
using EventBus.Messages.Results;
using Lend.API.Controllers.V1;
using Lend.API.Domain;
using Lend.API.Domain.Repositories;
using Lend.API.Domain.Strategies;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Lend.API.Handlers
{
    public class LendBasketCommandHandler : IRequestHandler<LendBasketCommand, LendBasketCommandResult>
    {
        private readonly IMemoryCache _cache;
        private readonly IRequestClient<GetCustomerInfo> _customerClient;
        private readonly ILendedBasketRepository _lendedBasketRepository;
        private readonly IEnumerable<IStrategy<SimpleIntRule>> _intStrategies;
        private readonly IEnumerable<IStrategy<SimpleBooleanRule>> _booleanStrategies;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public LendBasketCommandHandler(IMemoryCache cache, IRequestClient<GetCustomerInfo> customerClient, ILendedBasketRepository lendedBasketRepository, IEnumerable<IStrategy<SimpleIntRule>> intStrategies,
            IEnumerable<IStrategy<SimpleBooleanRule>> booleanStrategies, ISendEndpointProvider sendEndpointProvider)
        {
            _cache = cache;
            _customerClient = customerClient;
            _lendedBasketRepository = lendedBasketRepository;
            _intStrategies = intStrategies;
            _booleanStrategies = booleanStrategies;
            _sendEndpointProvider = sendEndpointProvider;
        }
        
        public async Task<LendBasketCommandResult> Handle(LendBasketCommand request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(request.EmployeeName, out Basket basket))
                return new LendBasketCommandResult(false, new List<string>() {"Basket not exists"});

            if (basket.Customer is null)
                return new LendBasketCommandResult(false, new List<string>() {"Customer is required for lend basket"});

            if (!basket.StockWithBooks.Any())
                return new LendBasketCommandResult(false, new List<string>() {"Stocks are required for lend basket"});
            
            //TODO check if customer exists
            var customerMessageResponse = await _customerClient.GetResponse<CustomerInfoResult>(
                new {Email = basket.Customer.Email.EmailAddress}, cancellationToken);

            if (customerMessageResponse.Message.Customer is null)
                return new LendBasketCommandResult(false,
                    new List<string>() {$"Customer with email {basket.Customer.Email.EmailAddress} not exist"});
            
            //TODO check if all stocks exists
            // var stockMessageResponse = await _stockClient.GetResponse<StocksExistanceResult>(new
            // {
            //     StocksIds = basket.StockWithBooks.Select(s => s.StockId).ToList()
            // });

            // if (!stockMessageResponse.Message.IsAllExists)
            //     return new LendBasketCommandResult(false, new List<string>() {"Not all Stocks exists in system"});
            
            //TODO this code is unnecasery because based on next call we are removing stock for this basket, so we know if it was successful then it wasnt lended by someone
            // var stocksInBasket = basket.StockWithBooks.Select(s => s.StockId).ToList();
            // var borrowedStocks  = (await _lendedBasketRepository.GetAllByStocksIds(stocksInBasket)).ToList();
            // if (borrowedStocks.Any())
            //     return new LendBasketCommandResult(false,
            //         new List<string>() {$"Stocks:{string.Join(",", borrowedStocks.Select(s => s.Stocks))} are already borrowed by someone"});

            var check = await CheckIfCustomerAndBasketMatchAllStrategies(basket);
            if (!check.Item1)
                return new LendBasketCommandResult(false, new List<string>() {check.Item2.FirstOrDefault()});

            var lendedBasket = new LendedBasket(basket, _intStrategies, _booleanStrategies);
            
            //TODO check if all stocks exists and remove them!
            // var stockMessageResponse = await _stockClient.GetResponse<DeleteStocksResult>(new
            // {
            //     StocksIds = basket.StockWithBooks.Select(s => s.StockId).ToList()
            // });
            var stocksIdsToRemove = basket.StockWithBooks.Select(s => s.StockId).ToList();
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{EventBusConstants.DeleteStocks}"));
            
            await endpoint.Send(new DeleteStocks() {StocksIds = stocksIdsToRemove}, cancellationToken);

            
            _lendedBasketRepository.Add(lendedBasket);
            if (await _lendedBasketRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 0)
                return new LendBasketCommandResult(false, new List<string>() {"Something went wrong during saving"});
            
            _cache.Remove(request.EmployeeName);
            return new LendBasketCommandResult(true);
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
    }
}