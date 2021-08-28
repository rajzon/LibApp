using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using StockDelivery.API.Commands.V1;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Domain;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Handlers
{
    public class ScanItemDeliveryCommandHandler : IRequestHandler<ScanItemDeliveryCommand, ScanItemDeliveryCommandResult >
    {
        private readonly IActiveDeliveryRepository _activeDeliveryRepository;
        private readonly IMapper _mapper;

        public ScanItemDeliveryCommandHandler(IActiveDeliveryRepository activeDeliveryRepository, IMapper mapper)
        {
            _activeDeliveryRepository = activeDeliveryRepository;
            _mapper = mapper;
        }
        
        public async Task<ScanItemDeliveryCommandResult> Handle(ScanItemDeliveryCommand request, CancellationToken cancellationToken)
        {
            var deliveryToScan = await _activeDeliveryRepository.FindByIdAsync(request.Id);

            if (deliveryToScan is null)
                return new ScanItemDeliveryCommandResult(false,
                    new[] {$"Delivery Item: {request.Id} not found."});

            if (!deliveryToScan.IsScanOperationAllowed(request.ScanMode, request.RequestedEan, out List<string> errors))
                return new ScanItemDeliveryCommandResult(false, errors);

            if (request.ScanMode)
                deliveryToScan.ScanItem(request.RequestedEan);
            else
                deliveryToScan.UnscanItem(request.RequestedEan);
            
            
            if (await _activeDeliveryRepository.UnitOfWork.SaveChangesAsync(cancellationToken) < 1)
                return new ScanItemDeliveryCommandResult(false,
                    new[] {$"Delivery Item: {request.Id} not found."});

            var activeDeliveryWithDesc = new ActiveDeliveryScanInfoDto()
            {
                ActiveDeliveryInfo = _mapper.Map<ActiveDeliveryDto>(deliveryToScan),
                Items = _mapper.Map<IEnumerable<ActiveDeliveryScanItemInfoDto>>(deliveryToScan.Items),
            };

            return new ScanItemDeliveryCommandResult(true, request.ScanMode, activeDeliveryWithDesc);

        }
    }
}