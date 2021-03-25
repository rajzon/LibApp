using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Book.API.Commands.V1;
using Book.API.Queries.V1;
using Book.API.Queries.V1.Dtos;
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
        [ProducesResponseType(typeof(CreateBookCommandResult), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            var result = await _mediator.Send(command);
            
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new FindBookByIdQuery {Id = id});

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBooksQuery());

            if (!result.Any())
                return NotFound();

            return Ok(result);
        }
        
    }

    public class GetAllBooksQuery : IRequest<IEnumerable<BookDto>>
    {
    }
}