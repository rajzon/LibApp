using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using Nest;
using Search.API.Application.Services;
using Search.API.Commands.V1;
using Search.API.Contracts.Responses;

namespace Search.API.Consumers
{
    public class GetCustomerInfoConsumer : IConsumer<GetCustomerInfo>
    {
        private readonly ISearchRepository _searchRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCustomerInfoConsumer> _logger;

        public GetCustomerInfoConsumer(ISearchRepository searchRepository, IMapper mapper, ILogger<GetCustomerInfoConsumer> logger)
        {
            _searchRepository = searchRepository;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<GetCustomerInfo> context)
        {
            _logger.LogInformation("GetCustomerInfoConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);
            
            var result = await _searchRepository.SearchCustomersByEmail(new SearchCustomerCommand() {SearchTerm = context.Message.Email});

            if (!result.Documents.Any())
            {
                _logger.LogError("GetCustomerInfoConsumer: Message {MessageId} failed to get Customer with email: {CustomerEmail}", context.MessageId, context.Message.Email);
                await context.RespondAsync<CustomerInfoResult>(new
                {
                    Customer = (CustomerBasketBusResponse)null
                });
            }
            
            await context.RespondAsync<CustomerInfoResult>(new
            {
                Customer = _mapper.Map<CustomerBasketBusResponse>(result.Documents.First())
            });
        }
    }
}