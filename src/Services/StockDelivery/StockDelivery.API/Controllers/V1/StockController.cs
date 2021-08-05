using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StockDelivery.API.Controllers.V1
{
    
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = "employee")]
    [Route("v{version:apiVersion}/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllStocksQuery());
            
            if (!result.Any())
                return NotFound();

            return Ok(result);
        }
    }

    public class GetAllStocksQuery : IRequest<IEnumerable<StockDto>>
    {
    }

    public class StockDto
    {
        public int Id { get; init; }
        public int BookId { get; init; }
        public string Ean { get; init; }
    }
}