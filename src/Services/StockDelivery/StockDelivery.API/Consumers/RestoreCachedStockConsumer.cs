using System.Collections.Concurrent;
using System.Threading.Tasks;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Consumers
{
    public class RestoreCachedStockConsumer : IConsumer<RestoreCachedStock>
    {
        private readonly IMemoryCache _cache;
        private readonly IBookStockRepository _bookStockRepository;
        private readonly ILogger<RestoreCachedStockConsumer> _logger;

        public RestoreCachedStockConsumer(IMemoryCache cache, IBookStockRepository bookStockRepository, ILogger<RestoreCachedStockConsumer> logger)
        {
            _cache = cache;
            _bookStockRepository = bookStockRepository;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<RestoreCachedStock> context)
        {
            _logger.LogInformation("RestoreCachedStockConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);
            if (!_cache.TryGetValue(context.Message.Ean, out ConcurrentQueue<int> stocks))
            {
                _logger.LogError("RestoreCachedStockConsumer: For Message {MessageId}, requested ean {Ean} to restore stocks not found in cache", context.MessageId, context.Message.Ean);
                await context.RespondAsync<RestoreCachedStockResult>(new
                {
                    IsSuccessful = false
                });
            }

            if (! await _bookStockRepository.IsExist(context.Message.StockToRestore))
            {
                _logger.LogError("RestoreCachedStockConsumer: For Message {MessageId}, requested stock to restore {StockToRestore} not found in Db", context.MessageId, context.Message.StockToRestore);
                await context.RespondAsync<RestoreCachedStockResult>(new
                {
                    IsSuccessful = false
                });
            }
            else
            {
                stocks.Enqueue(context.Message.StockToRestore);
                await context.RespondAsync<RestoreCachedStockResult>(new
                {
                    IsSuccessful = true
                });
            }


        }
    }
}