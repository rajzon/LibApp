using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using StockDelivery.API.Services.Results;

namespace StockDelivery.API.Services
{
    public class AccessContentService : IAccessContentService
    {
        public Dictionary<int,int> UserIdWithContentId { get; private set; }

        public AccessContentService()
        {
            UserIdWithContentId = new Dictionary<int, int>();
        }
        
        public AccessContentResult RemoveUserWithContent(HttpContext context, int contentId)
        {
            if (!TryParseSubClaim(context, out var userId))
                return new AccessContentResult(false, new []{ "Error occured during parsing UserId Claim from token"});
            
            var contentExist = UserIdWithContentId.TryGetValue(contentId, out int userIdFromStore);
            if (contentExist && userId.Equals(userIdFromStore))
            {
                UserIdWithContentId.Remove(contentId);
                return new AccessContentResult(true);
            }


            return new AccessContentResult(false, 
                new []{ "Requested Content not exist or you are trying to remove content that currently belongs to other User"});
        }

        public AccessContentResult CanUserAccessContent(HttpContext context, int contentId)
        {
            if (!TryParseSubClaim(context, out var userId))
                return new AccessContentResult(false, new []{ "Error occured during parsing UserId Claim from token"});
            
            var contentExist = UserIdWithContentId.TryGetValue(contentId, out int userIdFromStore);
            if (!contentExist)
            {
                //TODO: consider checking if contentId exists 
                UserIdWithContentId.Add(contentId, userId);
                return new AccessContentResult(true);
            }

            return userId.Equals(userIdFromStore) ? new AccessContentResult(true) : new AccessContentResult(false, 
                new []{ "Requested Content is currently opened by other User. Please try again later"});
        }
        
        private bool TryParseSubClaim(HttpContext context, out int userId)
        {
            if (!int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out userId))
                return false;
            return true;
        }
        
    }
}