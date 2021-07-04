using System.Collections.Generic;
using StockDelivery.API.Commands.V1.Common;
using StockDelivery.API.Commands.V1.Dtos;

namespace StockDelivery.API.Commands.V1
{
    public class CreateActiveDeliveryCommandResult : BaseCommandResult
    {
        public CommandActiveDeliveryDto ActiveDelivery { get; init; }
        
        public CreateActiveDeliveryCommandResult(bool succeeded, CommandActiveDeliveryDto activeDelivery) 
            : base(succeeded)
        {
            ActiveDelivery = activeDelivery;
        }

        public CreateActiveDeliveryCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default) 
            : base(succeeded, errors)
        {
        }
    }
}