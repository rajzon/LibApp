using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Domain;
using StockDelivery.API.Queries.V1;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Handlers
{
    public class GetAllStocksQueryHandler : IRequestHandler<GetAllStocksQuery, IEnumerable<StockDto>>
    {
        private readonly IBookStockRepository _bookStockRepository;
        private readonly IMapper _mapper;

        public GetAllStocksQueryHandler(IBookStockRepository bookStockRepository, IMapper mapper)
        {
            _bookStockRepository = bookStockRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<StockDto>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _bookStockRepository.GetAllAsync();
            
            var result =_mapper.Map<IEnumerable<StockDto>>(stocks);
            
            return result;
        }
    }
}