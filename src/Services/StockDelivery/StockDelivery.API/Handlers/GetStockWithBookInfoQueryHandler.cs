using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger<GetStockWithBookInfoQueryHandler> _logger;

        public GetStockWithBookInfoQueryHandler(IBookStockRepository bookStockRepository,
            IRequestClient<GetBookInfo> client, IMemoryCache cache, IMapper mapper, ILogger<GetStockWithBookInfoQueryHandler> logger)
        {
            _bookStockRepository = bookStockRepository;
            _client = client;
            _cache = cache;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<StockWithBookInfoDto> Handle(GetStockWithBookInfoQuery request, CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(request.BookEan, out ConcurrentQueue<int> stocks))
            {
                _logger.LogWarning("Missing in cache");
                return null;
            }

            if (!stocks.TryDequeue(out int stockId))
            {
                _logger.LogWarning("Problem during dequeue");
                return null;
            }
            
            var selectedStock = await _bookStockRepository.GetAsync(stockId);
            if (selectedStock is null)
            {
                _logger.LogWarning("Problem during get stockID");
                return null;
            }

            // _bookStockRepository.Remove(selectedStock);
            // if (await _bookStockRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 1)
            // {
            //     stocks.Enqueue(stockId);
            //     return null;
            // }
            
            var bookInfo = await _client.GetResponse<BookInfoResult>(
            new {BookId = selectedStock.BookId});

            if (bookInfo.Message.Result is null)
            {
                _logger.LogWarning("Problem during get book info");
                return null;
            }

            var result = _mapper.Map<StockWithBookInfoDto>(bookInfo.Message.Result);
            result.StockId = selectedStock.Id;

            return result;
        }
    }
}