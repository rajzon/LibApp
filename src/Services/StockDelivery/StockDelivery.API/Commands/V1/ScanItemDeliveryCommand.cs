using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Controllers.V1;

namespace StockDelivery.API.Commands.V1
{
    public class ScanItemDeliveryCommand : IRequest<ScanItemDeliveryCommandResult>
    {
        public int Id { get; set; }
        public bool ScanMode { get; init; } = true;
        public string RequestedEan { get; init; }
    }
}