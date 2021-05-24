using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Nest;
using Search.API.Domain;

namespace Search.API.Consumers
{
    public class CreateBookConsumer : IConsumer<CreateBook>
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<CreateBookConsumer> _logger;
        private readonly IMapper _mapper;


        public CreateBookConsumer(IElasticClient elasticClient,
            ILogger<CreateBookConsumer> logger,
            IMapper mapper)
        {
            _elasticClient = elasticClient;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task Consume(ConsumeContext<CreateBook> context)
        { 
           _logger.LogInformation($"CreateBookConsumer: Started Consuming Message {context.MessageId}");

           var message = _mapper.Map<Book>(context.Message);
           
           var response =  await _elasticClient.IndexDocumentAsync(message);

           if (! response.IsValid)
               _logger.LogError($"CreateBookConsumer: Message {context.MessageId} failed to insert data to Elasticsearch");
           else
               _logger.LogInformation($"CreateBookConsumer: Message {context.MessageId} successfully insert data to Elasticsearch");
        }
    }
}