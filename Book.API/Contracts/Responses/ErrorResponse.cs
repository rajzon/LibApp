using System.Collections.Generic;

namespace Book.API.Contracts.Responses
{
    public class ErrorResponse
    {
        public IReadOnlyCollection<string>  Errors { get; set; }
    }
}