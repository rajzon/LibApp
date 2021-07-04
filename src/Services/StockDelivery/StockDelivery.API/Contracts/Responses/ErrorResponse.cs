using System.Collections.Generic;

namespace StockDelivery.API.Contracts.Responses
{
    public class ErrorResponse
    {
        public IReadOnlyCollection<string>  Errors { get; set; }
    }
}