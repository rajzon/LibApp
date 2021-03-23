using System.Threading.Tasks;
using Book.API.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Book.API.Controllers.V1
{
    
    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator, ILogger<BookController> logger)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateBook(CreateBookCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}