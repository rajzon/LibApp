using System.Collections.Generic;

namespace StockDelivery.API.Services.Common
{
    public abstract class BaseServiceResult
    {
        public readonly bool Succeeded;
        public readonly IReadOnlyCollection<string> Errors;


        protected BaseServiceResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        protected BaseServiceResult(bool succeeded, IReadOnlyCollection<string> errors = default)
        {
            Succeeded = succeeded;
            Errors = errors ?? new List<string>();
        }
    }
}