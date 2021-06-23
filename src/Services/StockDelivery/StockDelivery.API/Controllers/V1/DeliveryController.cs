using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Commands.V1;
using StockDelivery.API.Commands.V1.Dtos;
using StockDelivery.API.Contracts.Responses;

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


        [HttpPost("create")]
        [ProducesResponseType(typeof(CommandActiveDeliveryDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateDelivery(CreateActiveDeliveryCommand command)
        {
            //1.1Call service and check if that id and Ean exists in Book Service
            //1.2  Create new Delivery with corresponding items
            var result = await _mediator.Send(command);
            
            //2 Grab result
            if (! result.Succeeded)
                return result.Errors.Any()? BadRequest(new ErrorResponse
                {
                    Errors =  result.Errors
                }): BadRequest();

            //3 Return result
            return Ok(result.ActiveDelivery);
        }
    }
}