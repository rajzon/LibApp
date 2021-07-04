using System.Collections.Generic;

namespace StockDelivery.API.Commands.V1.Common
{
    public abstract class BaseCommandResult
    {
        public readonly bool Succeeded;
        public readonly IReadOnlyCollection<string> Errors;


        protected BaseCommandResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        protected BaseCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default)
        {
            Succeeded = succeeded;
            Errors = errors ?? new List<string>();
        }
    }
}