using System.Threading.Tasks;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Consumers
{
    public class DeleteStocksConsumer : IConsumer<DeleteStocks>
    {
        private readonly IBookStockRepository _bookStockRepository;
        private readonly ILogger<DeleteStocksConsumer> _logger;

        public DeleteStocksConsumer(IBookStockRepository bookStockRepository, ILogger<DeleteStocksConsumer> logger)
        {
            _bookStockRepository = bookStockRepository;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<DeleteStocks> context)
        {
            _logger.LogInformation("DeleteStocksConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);
            foreach (var stockId in context.Message.StocksIds)
            {
                var stock = await _bookStockRepository.GetAsync(stockId);
                if (stock is null)
                {
                    _logger.LogError("DeleteStocksConsumer: For Message {MessageId}, requested stock to remove, with id {stockId} not found in DB", context.MessageId, stockId);
                    await context.RespondAsync<DeleteStocksResult>(new
                    {
                        IsAllRemoved = false
                    });
                }
                    
                
                _bookStockRepository.Remove(stock);
            }

            if (await _bookStockRepository.UnitOfWork.SaveChangesAsync() < 1)
            {
                _logger.LogError("DeleteStocksConsumer: For Message {MessageId} not all passed stocks removed from DB", context.MessageId);
                await context.RespondAsync<DeleteStocksResult>(new
                {
                    IsAllRemoved = false
                });

            }
            else
            {
                await context.RespondAsync<DeleteStocksResult>(new
                {
                    IsAllRemoved = true
                });
            }
            
              
                
            // if (!await _bookStockRepository.IsAllExists(context.Message.StocksIds))
            // {
            //     _logger.LogError("CheckStocksExistanceConsumer: For Message {MessageId} not all passed stocks exists in DB", context.MessageId);
            //     await context.RespondAsync<StocksExistanceResult>(new
            //     {
            //         IsAllExists = false
            //     }); 
            // }
                
            // else
            // {
            //     await context.RespondAsync<StocksExistanceResult>(new
            //     {
            //         IsAllExists = true
            //     });
            // }
        }
    }
}