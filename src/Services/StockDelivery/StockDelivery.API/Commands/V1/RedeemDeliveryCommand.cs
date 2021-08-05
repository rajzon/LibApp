using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Controllers.V1;
using StockDelivery.API.Queries.V1;

namespace StockDelivery.API.Commands.V1
{
    public class RedeemDeliveryCommand : IRequest<RedeemDeliveryCommandResult>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}