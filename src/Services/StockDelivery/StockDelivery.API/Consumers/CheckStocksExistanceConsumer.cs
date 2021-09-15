using System.Threading.Tasks;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Consumers
{
    public class CheckStocksExistanceConsumer : IConsumer<CheckStocksExistance>
    {
        private readonly IBookStockRepository _bookStockRepository;
        private readonly ILogger<CheckStocksExistanceConsumer> _logger;

        public CheckStocksExistanceConsumer(IBookStockRepository bookStockRepository, ILogger<CheckStocksExistanceConsumer> logger)
        {
            _bookStockRepository = bookStockRepository;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<CheckStocksExistance> context)
        {
            _logger.LogInformation("CheckStocksExistanceConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);
            if (!await _bookStockRepository.IsAllExists(context.Message.StocksIds))
            {
                _logger.LogError("CheckStocksExistanceConsumer: For Message {MessageId} not all passed stocks exists in DB", context.MessageId);
                await context.RespondAsync<StocksExistanceResult>(new
                {
                    IsAllExists = false
                }); 
            }
                
            else
            {
                await context.RespondAsync<StocksExistanceResult>(new
                {
                    IsAllExists = true
                });
            }
        }
    }
}