using Auth.Core.Entities;

namespace Auth.Infrastructure.Services.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(Account account);
    RefreshToken GenerateRefreshToken(Account account);
}