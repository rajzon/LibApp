using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Queries.V1;
using Book.API.Queries.V1.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Book.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AuthorDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllAuthorsQuery());
            var ctx = User.Claims;
            if (!result.Any())
                return NotFound();

            return Ok(result);
        }
    }
}