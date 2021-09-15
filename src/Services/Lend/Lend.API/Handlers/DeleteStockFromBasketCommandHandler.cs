using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Lend.API.Controllers.V1;
using Lend.API.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Lend.API.Handlers
{
    public class DeleteStockFromBasketCommandHandler : IRequestHandler<DeleteStockFromBasketCommand, PostBasketCommandResult>
    {
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public DeleteStockFromBasketCommandHandler(IMemoryCache cache, IMapper mapper)
        {
            _cache = cache;
            _mapper = mapper;
        }
        
        public async Task<PostBasketCommandResult> Handle(DeleteStockFromBasketCommand request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(request.EmployeeName, out Basket basket))
                return new PostBasketCommandResult(false, new List<string>() {"Basket not exists"});

            if (basket.StockWithBooks.All(s => s.StockId != request.StockId))
                return new PostBasketCommandResult(false,
                    new List<string>() {$"Stock with id {request.StockId} not exist in basket"});
            
            basket.RemoveStock(request.StockId);
            _cache.Set(request.EmployeeName, basket);
            
            return new PostBasketCommandResult(true, new BasketResponseDto()
            {
                Customer = basket.Customer is not null ? _mapper.Map<CustomerBasketDto>(basket.Customer) : null,
                StockWithBooks = _mapper.Map<IEnumerable<StockWithBooksBasketDto>>(basket.StockWithBooks),
            });
        }
    }
}