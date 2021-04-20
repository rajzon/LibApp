using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
    public class LanguageController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LanguageController> _logger;

        public LanguageController(IMediator mediator, ILogger<LanguageController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LanguageDto>), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllLanguagesQuery());
            var ctx = User.Claims;
            if (!result.Any())
                return NotFound();
            
            return Ok(result);
        }
        
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LanguageDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new FindLanguageByIdQuery {Id = id});

            if (result is null)
                return NotFound();
            
            return Ok(result);
        }
        
    }
}