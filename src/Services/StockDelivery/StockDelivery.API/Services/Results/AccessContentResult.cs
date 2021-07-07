using System.Collections.Generic;
using StockDelivery.API.Services.Common;

namespace StockDelivery.API.Services.Results
{
    public class AccessContentResult : BaseServiceResult
    {
        public AccessContentResult(bool succeeded) : base(succeeded)
        {
        }

        public AccessContentResult(bool succeeded, IReadOnlyCollection<string> errors = default) : base(succeeded, errors)
        {
        }
    }
}