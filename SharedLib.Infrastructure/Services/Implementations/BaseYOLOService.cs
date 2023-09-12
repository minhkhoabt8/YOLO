using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SharedLib.Infrastructure.DTOs;
using SharedLib.Infrastructure.Services.Interfaces;

namespace SharedLib.Infrastructure.Services.Implementations;

public class BaseYOLOService
{
    protected readonly HttpClient _httpClient;
    protected JsonSerializerOptions _jsonOptions = new() {PropertyNameCaseInsensitive = true};

    public BaseYOLOService(HttpClient httpClient, ITokenService tokenService, IConfiguration configuration)
    {
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokenService.GenerateToken());
        httpClient.BaseAddress = new Uri(configuration["BaseAddresses:DMSGateway"]);
        _httpClient = httpClient;
    }

    protected async Task<T> GetResultAsync<T>(HttpResponseMessage response)
    {
        return JsonSerializer.Deserialize<APIResult<T>>(
            await response.Content.ReadAsStringAsync(), _jsonOptions)!.Result;
    }
}