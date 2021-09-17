using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using MediatR;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Domain;
using StockDelivery.API.Queries.V1;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Handlers
{
    public class GetStockWithBookInfoQueryHandler : IRequestHandler<GetStockWithBookInfoQuery, StockWithBookInfoDto>
    {
        private readonly IBookStockRepository _bookStockRepository;
        private readonly IRequestClient<GetBookInfo> _client;
        private readonly IMapper _mapper;

        public GetStockWithBookInfoQueryHandler(IBookStockRepository bookStockRepository,
            IRequestClient<GetBookInfo> client, IMapper mapper)
        {
            _bookStockRepository = bookStockRepository;
            _client = client;
            _mapper = mapper;
        }
        
        public async Task<StockWithBookInfoDto> Handle(GetStockWithBookInfoQuery request, CancellationToken cancellationToken)
        {
            var selectedStock = await _bookStockRepository.GetAsync(request.StockId);
            
            if (selectedStock is null)
                return null;

            var bookInfo = await _client.GetResponse<BookInfoResult>(
                new {BookId = selectedStock.BookId});

            if (bookInfo.Message.Result is null)
                return null;

            var result = _mapper.Map<StockWithBookInfoDto>(bookInfo.Message.Result);
            result.StockId = selectedStock.Id;

            return result;
        }
    }
}