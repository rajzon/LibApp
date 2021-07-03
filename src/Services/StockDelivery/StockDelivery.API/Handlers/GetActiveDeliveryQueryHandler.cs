using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Domain;
using StockDelivery.API.Queries.V1;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Handlers
{
    public class GetActiveDeliveryQueryHandler : IRequestHandler<GetActiveDeliveryQuery, ActiveDeliveryWithItemsDto>
    {
        private readonly IActiveDeliveryRepository _activeDeliveryRepository;
        private readonly IRequestClient<GetBooksInfo> _client;
        private readonly IMapper _mapper;
        private readonly ILogger<GetActiveDeliveryQueryHandler> _logger;

        public GetActiveDeliveryQueryHandler(IActiveDeliveryRepository activeDeliveryRepository,
            IRequestClient<GetBooksInfo> client, IMapper mapper, ILogger<GetActiveDeliveryQueryHandler> logger)
        {
            _activeDeliveryRepository = activeDeliveryRepository;
            _client = client;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<ActiveDeliveryWithItemsDto> Handle(GetActiveDeliveryQuery request, CancellationToken cancellationToken)
        {
            var activeDelivery = await _activeDeliveryRepository.FindByIdAsync(request.Id);

            if (activeDelivery is null)
                return null;
            
            var bookInfoResult = await _client.GetResponse<BooksInfoResult>(
                new {BooksIds = activeDelivery.Items.Select(b => b.BookId).ToList()});
            

            var activeDeliveryWithDesc = new ActiveDeliveryWithItemsDto()
            {
                ActiveDeliveryInfo = _mapper.Map<ActiveDeliveryDto>(activeDelivery),
                Items = _mapper.Map<IEnumerable<ActiveDeliveryItemDto>>(activeDelivery.Items),
            };
            
            foreach (var item in activeDeliveryWithDesc.Items)
            {
                var a = bookInfoResult.Message.Results.FirstOrDefault(b => b.Ean13.Equals(item.BookEan));
                if(a is not null)
                    item.ItemDescription = _mapper.Map<ActiveDeliveryItemDescDto>(a);
            }


            return activeDeliveryWithDesc;
            
        }
    }
}