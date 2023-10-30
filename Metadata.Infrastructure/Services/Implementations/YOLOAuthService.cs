using Metadata.Infrastructure.DTOs.AccountMapping;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharedLib.Infrastructure.Services.Implementations;
using SharedLib.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Implementations
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
