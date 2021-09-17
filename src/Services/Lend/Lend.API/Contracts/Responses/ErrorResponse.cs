using System.Collections.Generic;

namespace Lend.API.Contracts.Responses
{
    public class ErrorResponse
    {
        public IReadOnlyCollection<string>  Errors { get; set; }
    }
}