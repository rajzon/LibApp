using System.Threading.Tasks;
using Book.API.Domain;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Book.API.Consumers
{
    public class CheckBooksExistanceConsumer : IConsumer<CheckBooksExsitance>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<CheckBooksExistanceConsumer> _logger;

        public CheckBooksExistanceConsumer(IBookRepository bookRepository, ILogger<CheckBooksExistanceConsumer> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<CheckBooksExsitance> context)
        {
            _logger.LogInformation("CheckBooksExistanceConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);
            
            var hasAllBooksExists = await _bookRepository.IsAllExistsAsync(context.Message.BooksIdsWithEans);

            await context.RespondAsync<BooksExitanceResult>(new
            {
                IsAllExists = hasAllBooksExists
            });
        }
    }
}