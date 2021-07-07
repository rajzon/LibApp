using Microsoft.AspNetCore.Http;
using StockDelivery.API.Services.Results;

namespace StockDelivery.API.Services
{
    public interface IAccessContentService
    {
        public AccessContentResult RemoveUserWithContent(HttpContext context, int contentId);
        public AccessContentResult CanUserAccessContent(HttpContext context, int contentId);
    }
}