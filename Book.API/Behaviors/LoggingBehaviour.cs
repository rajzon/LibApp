using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Book.API.Behaviors
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("-------- Handling command {CommandName} : {Command} --------", 
                request.GetType().Name,request);
            
            
            foreach (var property in  request.GetType().GetProperties())
            {
                _logger.LogInformation("{Property}: {Value}", property.Name,
                    property.GetValue(request));
            }

            var response = await next();


            _logger.LogInformation("-------- Command {CommandName} handled : response {response} {@responseObj} --------", 
                request.GetType().Name, response, response);

            return response;
        }
    }
}