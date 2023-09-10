using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace OcelotGW.Middlewares;

public class TokenTransformMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public TokenTransformMiddleware(RequestDelegate next, IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _next = next;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!(context.Request.Path.Value?.TrimEnd('/').EndsWith("map-token") ?? false) &&
            context.Request.Headers.Authorization.Any())
        {
            var jwtString = context.Request.Headers["Authorization"].ToString().Split(" ").Last();

            JwtSecurityToken jwtToken;

            try
            {
                jwtToken = new JwtSecurityToken(context.Request.Headers["Authorization"].ToString().Split(" ").Last());
            }
            catch
            {
                // Invalid token format
                await _next(context);
                return;
            }


            // Not DMS token
            if (!jwtToken.Issuer.StartsWith("DMS"))
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtString);

                var response = await client.PostAsync($"{_configuration["BaseAddresses:DMSGateway"]}/auth/map-token",
                    null);

                var dmsToken = await response.Content.ReadAsStringAsync();

                context.Request.Headers["Authorization"] = $"Bearer {dmsToken}";
            }
        }

        await _next(context);
    }
}