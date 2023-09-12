using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedLib.Infrastructure.Services.Implementations;

namespace SharedLib.Authorization;

public class AuthorizeInternalServiceFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Claims.Any(cl =>
                cl.Type == ClaimTypes.NameIdentifier && cl.Value == JWTTokenService.SYSTEM_ACCOUNT_ID.ToString()))
        {
            context.Result = new ForbidResult();
        }
    }
}