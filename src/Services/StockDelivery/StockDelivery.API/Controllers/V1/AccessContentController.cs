using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockDelivery.API.Contracts.Responses;
using StockDelivery.API.Queries.V1;
using StockDelivery.API.Queries.V1.Dtos;
using StockDelivery.API.Services;

namespace StockDelivery.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = "employee")]
    [Route("v{version:apiVersion}/[controller]")]
    public class AccessContentController : ControllerBase
    {
        private readonly IAccessContentService _accessContentService;

        public AccessContentController(IAccessContentService accessContentService)
        {
            _accessContentService = accessContentService;
        }
        
        [HttpGet("active-delivery/{contentId}/edit")]
        [Authorize(Policy = "delivery-edit")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public IActionResult CanUserAccessActiveDeliveryEditContent(int contentId)
        {
            var result = _accessContentService.CanUserAccessContent(HttpContext, contentId);

            if (!result.Succeeded)
                return result.Errors.Any()
                    //TODO: return something Positive(ex. NoConent) 
                    ? BadRequest(new ErrorResponse
                    {
                        Errors = result.Errors
                    }) : BadRequest();


            return Ok();
        }
        
        [HttpDelete("active-delivery/{contentId}/edit/remove-access")]
        [Authorize(Policy = "delivery-edit")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public IActionResult RemoveUserAccessToActiveDeliveryEditContent(int contentId)
        {
            var result = _accessContentService.RemoveUserWithContent(HttpContext, contentId);
            
            if (!result.Succeeded)
                return result.Errors.Any()
                    //TODO: return something Positive(ex. NoConent) 
                    ? BadRequest(new ErrorResponse
                    {
                        Errors = result.Errors
                    }) : BadRequest();

            return Ok();
        }
    }
}