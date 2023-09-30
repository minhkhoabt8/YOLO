using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SharedLib.Infrastructure.Services.Interfaces;

namespace SharedLib.Infrastructure.Services.Implementations
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string? AccountID =>
                _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier)?.Value ?? null;
                
        public string? Username =>
            _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Name)?.Value ?? null;

        public string? Email =>
            _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email)?.Value ?? null;

        public bool IsAuthenticated => _contextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}