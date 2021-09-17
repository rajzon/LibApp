using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Search.API.Domain;

namespace Search.API.Consumers
{
    public class CreateSeededCustomersConsumer : IConsumer<CreateSeededCustomers>
    {
        private readonly IMapper _mapper;
        private readonly IElasticClient _elasticClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateSeededCustomersConsumer> _logger;

        public CreateSeededCustomersConsumer(IMapper mapper, IElasticClient elasticClient,
            IConfiguration configuration, ILogger<CreateSeededCustomersConsumer> logger)
        {
            _mapper = mapper;
            _elasticClient = elasticClient;
            _configuration = configuration;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<CreateSeededCustomers> context)
        {
            _logger.LogInformation("CreateSeededCustomersConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);
            
            var messageCustomers = _mapper.Map<IEnumerable<Customer>>(context.Message.Customers);

            foreach (var customer in messageCustomers)
            {
                customer.NameSuggest = new CompletionField() {Input = customer.Name.Split()};
                customer.SurnameSuggest = new CompletionField() {Input = customer.Surname.Split()};
                customer.EmailSuggest = new CompletionField() {Input = customer.Email.EmailAddress.Split()};
            }
            var createIndexDescriptor = new CreateIndexDescriptor(_configuration["elasticsearch:customerIndexName"])
                .Mappings(ms => ms
                    .Map<Customer>(m => m
                        .AutoMap())
                );

            await _elasticClient.Indices.CreateAsync(createIndexDescriptor);
            var customersSrc = await _elasticClient.SearchAsync<Customer>(s => s
                .Index(_configuration["elasticsearch:customerIndexName"])
                .Query(q => q.MatchAll()));

            
            if (!customersSrc.Documents.Any())
            {
                var response = await 
                    _elasticClient.IndexManyAsync(messageCustomers, index:_configuration["elasticsearch:customerIndexName"]);
                
                if (!response.IsValid)
                    _logger.LogError("CreateSeededCustomersConsumer: Message {MessageId} failed to insert data to Elasticsearch", context.MessageId);
                else
                    _logger.LogInformation("CreateSeededCustomersConsumer: Message {MessageId} successfully inserted data to Elasticsearch", context.MessageId);
            }
            else 
                _logger.LogInformation("CreateSeededCustomersConsumer: Message {MessageId} skipped seeding data to Elasticsearch because Customer Index is not empty", context.MessageId);
            
        }
    }
}