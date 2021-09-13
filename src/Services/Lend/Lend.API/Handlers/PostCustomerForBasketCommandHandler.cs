using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using Lend.API.Controllers.V1;
using Lend.API.Domain;
using Lend.API.Domain.Repositories;
using Lend.API.Domain.Strategies;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using IdentityType = Lend.API.Domain.IdentityType;

namespace Lend.API.Handlers
{
    public class PostCustomerForBasketCommandHandler : IRequestHandler<PostCustomerForBasketCommand, PostBasketCommandResult>
    {
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly IRequestClient<GetCustomerInfo> _client;
        private readonly ILendedBasketRepository _lendedBasketRepository;
        private readonly IEnumerable<IStrategy<SimpleIntRule>> _intStrategies;
        private readonly IEnumerable<IStrategy<SimpleBooleanRule>> _booleanStrategies;

        public PostCustomerForBasketCommandHandler(IMemoryCache cache, IMapper mapper,
            IRequestClient<GetCustomerInfo> client, ILendedBasketRepository lendedBasketRepository,
            IEnumerable<IStrategy<SimpleIntRule>> intStrategies, IEnumerable<IStrategy<SimpleBooleanRule>> booleanStrategies)
        {
            _cache = cache;
            _mapper = mapper;
            _client = client;
            _lendedBasketRepository = lendedBasketRepository;
            _intStrategies = intStrategies;
            _booleanStrategies = booleanStrategies;
        }
        
        public async Task<PostBasketCommandResult> Handle(PostCustomerForBasketCommand request, CancellationToken cancellationToken)
        {
            //TODO check if customer exist in User service
            var messageResponse = await _client.GetResponse<CustomerInfoResult>(
                new {Email = request.CustomerEmail}, cancellationToken);

            var customerResponse = messageResponse.Message.Customer;

            if (customerResponse is null)
                return new PostBasketCommandResult(false, new List<string>() {"Customer is not found"});

            var errors = new List<string>();
            var warnings = new List<string>();
        
            //TODO check if user can borrow book if already have borrowed books (call strategy [bool]), then append error collection,(doesnt make ...
            //TODO ... sense because it is just case when max allowed books is set to 1)
            
            //TODO check if stockId is already lended to other customer, NOT needed here because responsibility to check that should be located in 
            //TODO ... posting stockId for basket 
            
            
            
            
            
            //TODO create Factory that will create Basket and LendedBasket, and call async calls in factory(not in ex. basket, lendedbasket)
            //TODO ... and inside factory create method that will return all streategy errors and warnings
            //TODO check if user is debtor,-> have already lended books which return date is overdue, then append error or warning collection...
            //TODO ... depend on system settings (call strategy [bool])
            
            var identityType = _mapper.Map<IdentityType>(customerResponse.IdentityType);
            var address = CreateAddressBasket(customerResponse);
            var correspondenceAddress = CreateAddressCorrespondenceBasket(customerResponse);

            var customer = CreateCustomerBasket(customerResponse, address, correspondenceAddress, identityType);

            if (!_cache.TryGetValue(request.UserName, out Basket basket))
            {
                basket = new Basket(customer, null);
                //TODO check if user have to many books lended(call strategy), then append error collection
                //TODO check if user have any books borrowed, then  append warning collection [business requirement]
                
                var check= await CheckIfCustomerMatchAllStrategies(basket);
                errors.AddRange(check.Item1);
                warnings.AddRange(check.Item2);
                _cache.Set(request.UserName, basket);
            }
            else
            {
                basket.AssignNewCustomer(customer);
                
                //TODO check if user have any books already lended and they are placed in basket, then append error collection [domain rule]
                var customerBaskets =  await _lendedBasketRepository.GetAllByCustomerEmail(request.CustomerEmail);
                var conflictedStock = new StringBuilder();
                foreach (var customerBasket in customerBaskets)
                {
                    if (customerBasket.HasAnyBooksInBasketAreAlreadyLendedByCustomer(basket, out List<int> conflictStocks))
                    {
                        conflictedStock.Append(string.Join(",", conflictStocks));
                    }
                }
                if (conflictedStock.Length > 0)
                    errors.Add($"Customer already have borrowed following stocks:{conflictedStock}");

                //TODO check if user have to many books lended(call strategy), then append error collection
                //TODO check if user have any books borrowed, then  append warning collection [business requirement]
                var potentialErrors= await CheckIfCustomerMatchAllStrategies(basket);
                errors.AddRange(potentialErrors.Item1);
                warnings.AddRange(potentialErrors.Item2);
                
                _cache.Set(request.UserName, basket);
            }

            //var response = _mapper.Map<BasketResponseDto>(basket);
            var response = new BasketResponseDto()
            {
                Customer = _mapper.Map<CustomerBasketDto>(basket.Customer),
                StockWithBooks = _mapper.Map<IEnumerable<StockWithBooksBasketDto>>(basket.StockWithBooks),
                BusinessErrors = errors,
                BusinessWarnings = warnings
            };

            return new PostBasketCommandResult(true, response);
        }

        private async Task<(List<string>, List<string>)> CheckIfCustomerMatchAllStrategies(Basket basket)
        {
            var errors = new List<string>();
            var warnings = new List<string>();
            
            foreach (var intStrategy in _intStrategies)
            {
                var match = await intStrategy.IsBasketMatchStrategy(basket);
                if (!match.Item1)
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
                if (!match.Item1)
                {
                    if (match.Item2.ErrorType == ErrorType.Error)
                        errors.Add(match.Item2.ErrorDescription);
                    else
                        warnings.Add(match.Item2.ErrorDescription);
                }
            }
            

            return (errors, warnings);
        }

        private AddressBasket CreateAddressBasket(CustomerBasketBusResponse customerResponse)
        {
            return new AddressBasket(customerResponse.Address.Adres, customerResponse.Address.City,
                new PostCodeBasket(customerResponse.Address.PostCode),
                customerResponse.Address.Post, customerResponse.Address.Country);
        }
        
        private AddressCorrespondenceBasket CreateAddressCorrespondenceBasket(CustomerBasketBusResponse customerResponse)
        {
            return new AddressCorrespondenceBasket(customerResponse.Address.Adres, customerResponse.Address.City,
                new PostCodeBasket(customerResponse.Address.PostCode),
                customerResponse.Address.Post, customerResponse.Address.Country);
        }

        private CustomerBasket CreateCustomerBasket(CustomerBasketBusResponse customerResponse,
            AddressBasket address,
            AddressCorrespondenceBasket correspondenceAddress, IdentityType identityType)
        {
            return new CustomerBasket(customerResponse.Id, customerResponse.Name, customerResponse.Surname, new EmailBasket(customerResponse.Email),
                new IdCardBasket(customerResponse.PersonIdCard, identityType), identityType, customerResponse.Nationality,
                customerResponse.DateOfBirth, address, correspondenceAddress);
        }
    }
}