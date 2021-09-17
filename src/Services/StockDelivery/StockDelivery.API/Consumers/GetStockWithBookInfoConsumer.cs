using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using StockDelivery.API.Queries.V1;

namespace StockDelivery.API.Consumers
{
    public class GetStockWithBookInfoConsumer : IConsumer<GetStockWithBookInfo>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GetStockWithBookInfoConsumer> _logger;
        private readonly IMapper _mapper;

        public GetStockWithBookInfoConsumer(IMediator mediator, ILogger<GetStockWithBookInfoConsumer> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task Consume(ConsumeContext<GetStockWithBookInfo> context)
        {
            _logger.LogInformation("GetStockWithBookInfoConsumer: Started Consuming Message {MessageId} : {@Message}",context.MessageId, context.Message);
            
            var result = await _mediator.Send(new GetStockWithBookInfoQuery() {StockId = context.Message.StockId});
            
            if (result is null)
            {
                _logger.LogError("GetStockWithBookInfoConsumer: Message {MessageId} failed to get Stock: {StockId}", context.MessageId, context.Message.StockId);
                await context.RespondAsync<StockWithBookInfoResult>(new
                {
                    StockWithBookInfo = (StockWithBookInfoBusResponse)null
                });
            }

            await context.RespondAsync<StockWithBookInfoResult>(new
            {
                StockWithBookInfo = _mapper.Map<StockWithBookInfoBusResponse>(result)
            });

        }
    }
}