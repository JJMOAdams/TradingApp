using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TradingApp.Entities;

namespace TradingApp.Filters
{
    public class ActiveAuthorizationFilter(UserManager<AppUser> userManager) : IAsyncAuthorizationFilter
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || !user.IsActive)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }  
    }
}