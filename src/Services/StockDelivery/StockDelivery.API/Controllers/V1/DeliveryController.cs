using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "employee")]
    [Route("v{version:apiVersion}/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("active")]
        [ProducesResponseType(typeof(ActiveDeliveryDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] short currentPage, [FromQuery] short pageSize)
        {
            var result = await _mediator.Send(new GetAllActiveDeliveriesQuery(currentPage, pageSize));

            if (!result.Result.Any())
                return NotFound();

            return Ok(result);
        }

        [HttpGet("active/{id}")]
        [ProducesResponseType(typeof(ActiveDeliveryWithItemsDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetActiveDeliveryQuery(id));

            if (result is null || !result.Items.Any() || result.ActiveDeliveryInfo is null)
                return NotFound();

            return Ok(result);
        }


        [HttpPost("active/create")]
        [Authorize(Policy = "delivery-create-delete")]
        [ProducesResponseType(typeof(CommandActiveDeliveryDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateActiveDelivery(CreateActiveDeliveryCommand command)
        {
            var result = await _mediator.Send(command);
            

            if (! result.Succeeded)
                return result.Errors.Any()? BadRequest(new ErrorResponse
                {
                    Errors =  result.Errors
                }): BadRequest();


            return Ok(result.ActiveDelivery);
        }


        [HttpDelete("active/delete/{deliveryId}")]
        [Authorize(Policy = "delivery-create-delete")]
        [ProducesResponseType(typeof(DeleteActiveDeliveryCommandResult), (int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteActiveDelivery(int deliveryId)
        {
            var result = await _mediator.Send(new DeleteActiveDeliveryCommand() {DeliveryId = deliveryId});

            if (! result.Succeeded)
                return result.Errors.Any()? BadRequest(new ErrorResponse
                {
                    Errors =  result.Errors
                }): BadRequest();

            return NoContent();
        }
    }
}