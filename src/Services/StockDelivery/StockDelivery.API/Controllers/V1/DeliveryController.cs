using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Commands.V1;
using StockDelivery.API.Commands.V1.Dtos;
using StockDelivery.API.Contracts.Responses;
using StockDelivery.API.Queries.V1;
using StockDelivery.API.Queries.V1.Dtos;

namespace StockDelivery.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ActiveDeliveryDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] short currentPage, [FromQuery] short pageSize)
        {
            var result = await _mediator.Send(new GetAllActiveDeliveriesQuery(currentPage, pageSize));

            if (!result.Result.Any())
                return NotFound();

            return Ok(result);
        }


        [HttpPost("create")]
        [ProducesResponseType(typeof(CommandActiveDeliveryDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateDelivery(CreateActiveDeliveryCommand command)
        {
            var result = await _mediator.Send(command);
            

            if (! result.Succeeded)
                return result.Errors.Any()? BadRequest(new ErrorResponse
                {
                    Errors =  result.Errors
                }): BadRequest();


            return Ok(result.ActiveDelivery);
        }
    }
}