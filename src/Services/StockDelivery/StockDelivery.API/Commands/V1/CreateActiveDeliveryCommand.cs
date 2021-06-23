using System.Collections.Generic;
using MediatR;
using StockDelivery.API.Commands.V1.Dtos;
using StockDelivery.API.Controllers.V1;

namespace StockDelivery.API.Commands.V1
{
    public class CreateActiveDeliveryCommand : IRequest<CreateActiveDeliveryCommandResult>
    {
        public string Name { get; init; }
        public IReadOnlyCollection<DeliveryItemInfoDto> ItemsInfo { get; init; }
    }
}