using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Commands.V1;
using StockDelivery.API.Commands.V1.Common;
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

        [HttpPut("active/{id}")]
        [Authorize(Policy = "delivery-edit")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> EditActiveDelivery(EditActiveDeliveryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return result.Errors.Any()
                    ? BadRequest(new ErrorResponse
                    {
                        Errors = result.Errors
                    }) : BadRequest();

            return Ok();
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

    public class EditActiveDeliveryCommand : IRequest<EditActiveDeliveryCommandResult>
    {
        [FromRoute]
        public int Id { get; init; }
        public IEnumerable<ActiveDeliveryItemForEditDto> Items { get; init; }
    }

    public class ActiveDeliveryItemForEditDto
    {
        public int ItemId { get; init; }
        public int BookId { get; init; }
        public string BookEan { get; init; }
        public short ItemsCount { get; init; }
    }
    
    public class EditActiveDeliveryCommandResult : BaseCommandResult
    {
        public EditActiveDeliveryCommandResult(bool succeeded) 
            : base(succeeded)
        {
        }

        public EditActiveDeliveryCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default) 
            : base(succeeded, errors)
        {
        }
    }
}