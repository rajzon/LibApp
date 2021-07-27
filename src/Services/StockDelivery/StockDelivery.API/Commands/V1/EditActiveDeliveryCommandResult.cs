using System.Collections.Generic;
using StockDelivery.API.Commands.V1.Common;

namespace StockDelivery.API.Commands.V1
{
    public class EditActiveDeliveryCommandResult : BaseCommandResult
    {
        public EditActiveDeliveryCommandResult(bool succeeded) 
            : base(succeeded)
        {
        }

        public EditActiveDeliveryCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default) 
            : base(succeeded, errors)
        {
        }
    }
}