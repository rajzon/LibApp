using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Lend.API.Controllers.V1;
using Lend.API.Domain;
using Lend.API.Domain.Strategies;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Lend.API.Handlers
{
    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, BasketResponseDto>
    {
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly IEnumerable<IStrategy<SimpleIntRule>> _intStrategies;
        private readonly IEnumerable<IStrategy<SimpleBooleanRule>> _booleanStrategies;

        public GetBasketQueryHandler(IMemoryCache cache, IMapper mapper, IEnumerable<IStrategy<SimpleIntRule>> intStrategies,
            IEnumerable<IStrategy<SimpleBooleanRule>> booleanStrategies)
        {
            _cache = cache;
            _mapper = mapper;
            _intStrategies = intStrategies;
            _booleanStrategies = booleanStrategies;
        }
        
        public async Task<BasketResponseDto> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(request.EmployeeName, out Basket basket))
                return null;
            
            
            var check = await CheckIfCustomerAndBasketMatchAllStrategies(basket);
            
            return new BasketResponseDto()
            {
                Customer = basket.Customer is not null ? _mapper.Map<CustomerBasketDto>(basket.Customer) : null,
                StockWithBooks = _mapper.Map<IEnumerable<StockWithBooksBasketDto>>(basket.StockWithBooks),
                BusinessErrors = check.Item2,
                BusinessWarnings = check.Item3
            };
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