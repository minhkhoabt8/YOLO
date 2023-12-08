using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharedLib.Infrastructure.Services.Implementations;
using SharedLib.Infrastructure.Services.Interfaces;
using System.Net.Http.Headers;
using Signature.Infrastructure.Services.Interfaces;
using Signature.Infrastructure.DTOs.AccountMapping;

namespace Signature.Infrastructure.Services.Implementation
{
    public class YOLOAuthService : BaseYOLOService, IAuthService
    {
        public YOLOAuthService(HttpClient httpClient, ITokenService tokenService, IConfiguration configuration
            , IHttpContextAccessor contextAccessor) : base(httpClient, tokenService, configuration)
        {
            httpClient.DefaultRequestHeaders.Authorization =
            contextAccessor.HttpContext.User.Identity?.IsAuthenticated ?? false
                ? new AuthenticationHeaderValue("Bearer",
                    contextAccessor.HttpContext.Request.Headers["Authorization"].Single().Split(" ")[^1])
                : null;
        }

        public async Task<AccountMappping> GetAccountByIdAsync(string Id)
        {
            var response = await _httpClient.GetAsync($"auth/account/{string.Join("&ids=", Id)}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"YoloAuth: {response.ReasonPhrase}");
            }

            return await GetResultAsync<AccountMappping>(response);
        }
    }
}
