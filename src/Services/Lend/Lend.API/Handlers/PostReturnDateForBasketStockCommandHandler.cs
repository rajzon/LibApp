using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Lend.API.Controllers.V1;
using Lend.API.Domain;
using Lend.API.Domain.Strategies;
using Lend.API.Extenstions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Lend.API.Handlers
{
    public class PostReturnDateForBasketStockCommandHandler : IRequestHandler<PostReturnDateForBasketStockCommand, PostBasketCommandResult>
    {
        private readonly IMemoryCache _cache;
        private readonly IEnumerable<IStrategy<SimpleIntRule>> _intStrategies;
        private readonly IMapper _mapper;

        public PostReturnDateForBasketStockCommandHandler(IMemoryCache cache, IEnumerable<IStrategy<SimpleIntRule>> intStrategies, IMapper mapper)
        {
            _cache = cache;
            _intStrategies = intStrategies;
            _mapper = mapper;
        }
        
        public async Task<PostBasketCommandResult> Handle(PostReturnDateForBasketStockCommand request, CancellationToken cancellationToken)
        {
            //TODO FOR Existing Stocks: check if passed return date is not exceeding max allowed by organization(call strategy), or Call strategy
            //TODO... during creation of Basket and then fire STRATEGY
            if (!_cache.TryGetValue(request.EmployeeName, out Basket basket))
                return new PostBasketCommandResult(false, new List<string>() {"Basket not exists"});

            
            if (basket.StockWithBooks.All(s => s.StockId != request.StockId))
                return new PostBasketCommandResult(false,
                    new List<string>() {$"Stock with id{request.StockId} not exists in basket"});
            
            var basketToEdit = basket.DeepCopy();
            var check = await basketToEdit.TryEditReturnDateOfStock(request.StockId, request.ReturnDate, _intStrategies);
            if (!check.Item1)
                return new PostBasketCommandResult(false, new List<string>() {check.Item2.ErrorDescription});

            _cache.Remove(request.EmployeeName);
            _cache.Set(request.EmployeeName, basketToEdit);

            return new PostBasketCommandResult(true, new BasketResponseDto()
            {
                Customer = _mapper.Map<CustomerBasketDto>(basketToEdit.Customer),
                StockWithBooks = _mapper.Map<IEnumerable<StockWithBooksBasketDto>>(basketToEdit.StockWithBooks)
            });
        }
    }
}