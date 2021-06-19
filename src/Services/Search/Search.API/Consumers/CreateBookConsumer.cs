using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;


        public CreateBookConsumer(IElasticClient elasticClient,
            ILogger<CreateBookConsumer> logger,
            IMapper mapper, IConfiguration configuration)
        {
            _elasticClient = elasticClient;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }
        
        public async Task Consume(ConsumeContext<CreateBook> context)
        { 
            _logger.LogInformation("CreateBookConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);
            
            
           var message = _mapper.Map<Book>(context.Message);
           message.TitleSuggest = new CompletionField() { Input = message.Title.Split() };
           message.AuthorSuggest = new CompletionField() {Input = AuthorsToIndex(message) };
           message.EanSuggest = new CompletionField() {Input = message.Ean13.Split()};

           var response =  await _elasticClient.IndexAsync(message, i => i.Index(_configuration["elasticsearch:bookIndexName"]));

           if (! response.IsValid)
               _logger.LogError("CreateBookConsumer: Message {MessageId} failed to insert data to Elasticsearch", context.MessageId);
           else
               _logger.LogInformation("CreateBookConsumer: Message {MessageId} successfully inserted data to Elasticsearch", context.MessageId);
        }

        private static IEnumerable<string> AuthorsToIndex(Book message)
        {
            var authors = message.Authors.Select(a => a.Name.FullName);
            var authorsToIndex = new List<string>();
            foreach (var author in authors)
            {
                authorsToIndex.AddRange(author.Split());
            }

            return authorsToIndex;
        }
    }
}