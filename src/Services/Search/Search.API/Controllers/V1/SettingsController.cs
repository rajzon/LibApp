using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Search.API.Application.Services.Common;

namespace Search.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = "employee")]
    [Route("v{version:apiVersion}/[controller]")]
    public class SettingsController : ControllerBase
    {
        
        [HttpGet("book-management/allowed-sorting")]
        [ProducesResponseType(typeof(IReadOnlyCollection<AllowedSortingResponse>), (int) HttpStatusCode.OK)]
        public IActionResult GetSearchBookAllowedSorting()
        {
            var response = BookRepositorySettings.BOOK_MANAGEMENT_SORT_ALLOWED_VALUES
                .Select(s => new AllowedSortingResponse {Field = s.Key}).ToList();
            
            if (! response.Any())
                return NotFound();
            
            return Ok(response);
        }
    }

    public class AllowedSortingResponse
    {
        public string Field { get; init; }
    }
}