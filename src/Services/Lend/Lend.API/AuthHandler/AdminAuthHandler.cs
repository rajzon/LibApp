using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Lend.API.AuthHandler
{
    public class AdminAuthHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach (var pendingRequirement in pendingRequirements)
            {
                if (context.User.IsInRole("admin"))
                { 
                    context.Succeed(pendingRequirement);
                }
            }
            
            return Task.CompletedTask;
        }
    }
}