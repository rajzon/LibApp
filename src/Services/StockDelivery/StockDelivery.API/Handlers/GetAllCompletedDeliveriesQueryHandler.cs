using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Domain;
using StockDelivery.API.Queries.V1;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Handlers
{
    public class GetAllCompletedDeliveriesQueryHandler : IRequestHandler<GetAllCompletedDeliveriesQuery, IEnumerable<CompletedDeliveryDto>>
    {
        private readonly ICompletedDeliveryRepository _completedDeliveryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllCompletedDeliveriesQueryHandler> _logger;

        public GetAllCompletedDeliveriesQueryHandler(ICompletedDeliveryRepository completedDeliveryRepository, IMapper mapper,
            ILogger<GetAllCompletedDeliveriesQueryHandler> logger)
        {
            _completedDeliveryRepository = completedDeliveryRepository;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<IEnumerable<CompletedDeliveryDto>> Handle(GetAllCompletedDeliveriesQuery request, CancellationToken cancellationToken)
        {
            var deliveries = await _completedDeliveryRepository.GetAllAsync();
            
            _logger.LogError("{@test}", deliveries.FirstOrDefault()?.Items);
            var result = _mapper.Map<IEnumerable<CompletedDeliveryDto>>(deliveries);

            return result;
        }
    }
}