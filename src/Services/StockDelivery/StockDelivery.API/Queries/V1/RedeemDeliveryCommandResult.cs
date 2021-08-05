using System.Collections.Generic;
using StockDelivery.API.Commands.V1.Common;

namespace StockDelivery.API.Queries.V1
{
    public class RedeemDeliveryCommandResult : BaseCommandResult
    {
        public RedeemDeliveryCommandResult(bool succeeded) : base(succeeded)
        {
        }

        public RedeemDeliveryCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default) : base(succeeded, errors)
        {
        }
    }
}