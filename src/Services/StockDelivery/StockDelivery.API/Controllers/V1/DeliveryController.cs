using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Commands.V1;
using StockDelivery.API.Commands.V1.Dtos;
using StockDelivery.API.Contracts.Responses;
using StockDelivery.API.Domain;
using StockDelivery.API.Queries.V1;
using StockDelivery.API.Queries.V1.Dtos;
using StockDelivery.API.Services;

namespace StockDelivery.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = "employee")]
    [Route("v{version:apiVersion}/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAccessContentService _accessContentService;

        public DeliveryController(IMediator mediator, IAccessContentService accessContentService)
        {
            _mediator = mediator;
            _accessContentService = accessContentService;
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
        
        [HttpGet("completed")]
        [ProducesResponseType(typeof(CompletedDeliveryDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllCompleted()
        {
            var result = await _mediator.Send(new GetAllCompletedDeliveriesQuery());

            if (!result.Any())
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
            var accessResult = _accessContentService.CanUserAccessContent(HttpContext, command.Id);
            if (!accessResult.Succeeded)
                return accessResult.Errors.Any()
                    ? BadRequest(new ErrorResponse
                    {
                        Errors = accessResult.Errors
                    }) : BadRequest();
            
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
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteActiveDelivery(int deliveryId, string cancellationReason = default)
        {
            var result = await _mediator.Send(new DeleteActiveDeliveryCommand() {DeliveryId = deliveryId, CancellationReason = cancellationReason});

            if (! result.Succeeded)
                return result.Errors.Any()? BadRequest(new ErrorResponse
                {
                    Errors =  result.Errors
                }): BadRequest();

            return NoContent();
        }

        [HttpPost("{id}/scan")]
        [Authorize(Policy = "delivery-redeem")]
        [ProducesResponseType(typeof(object), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ScanBook(ScanItemDeliveryCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (!result.Succeeded)
                return result.Errors.Any()
                    ? BadRequest(new ErrorResponse
                    {
                        Errors = result.Errors
                    }) : BadRequest();

            return Ok(new {result.ScanMode});
        }


        [HttpDelete("{id}/redeem")]
        [Authorize(Policy = "delivery-redeem")]
        public async Task<IActionResult> RedeemDelivery(RedeemDeliveryCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (!result.Succeeded)
                return result.Errors.Any()
                    ? BadRequest(new ErrorResponse
                    {
                        Errors = result.Errors
                    }) : BadRequest();

            return NoContent();
        }
    }
}