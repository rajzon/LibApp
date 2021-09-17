using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Queries.V1;
using StockDelivery.API.Queries.V1.Dtos;

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

        [HttpGet("{stockId}/with-book-info")]
        [ProducesResponseType(typeof(StockWithBookInfoDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetWithBookInfo(int stockId)
        {
            var result = await _mediator.Send(new GetStockWithBookInfoQuery() {StockId = stockId});

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}