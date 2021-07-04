using System.Collections.Generic;
using StockDelivery.API.Commands.V1.Common;

namespace StockDelivery.API.Commands.V1
{
    public class DeleteActiveDeliveryCommandResult : BaseCommandResult
    {
        
        public DeleteActiveDeliveryCommandResult(bool succeeded) 
            : base(succeeded)
        {
        }

        public DeleteActiveDeliveryCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default) 
            : base(succeeded, errors)
        {
        }
    }
}