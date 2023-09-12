using System.Security.Claims;

namespace SharedLib.Infrastructure.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(params Claim[] claims);
}