using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Commands.V1.Dtos;
using StockDelivery.API.Controllers.V1;

namespace StockDelivery.API.Commands.V1
{
    public class EditActiveDeliveryCommand : IRequest<EditActiveDeliveryCommandResult>
    {
        [FromRoute]
        public int Id { get; init; }
        public IEnumerable<ActiveDeliveryItemForEditDto> Items { get; init; }
    }
}