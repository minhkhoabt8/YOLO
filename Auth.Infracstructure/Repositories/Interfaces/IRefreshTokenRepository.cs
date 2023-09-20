using Auth.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;

namespace Auth.Infrastructure.Repositories.Interfaces;

public interface IRefreshTokenRepository :
    IAddAsync<RefreshToken>
{
    Task<RefreshToken?> FindByTokenAsync(string token);
    Task<RefreshToken?> FindByTokenIncludeAccountAsync(string? token);
}