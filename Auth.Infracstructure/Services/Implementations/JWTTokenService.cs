using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Core.Entities;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Services.Implementations;

public class JWTTokenService : ITokenService
{
    private readonly IRoleService _roleService;
    private readonly IConfiguration _configuration;

    public JWTTokenService(IRoleService roleService, IConfiguration configuration)
    {
        _roleService = roleService;
        _configuration = configuration;
    }

    public RefreshToken GenerateRefreshToken(Account account)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            // Last for a month
            Expires = DateTime.UtcNow.AddMinutes(45),
            AccountId = account.Id
        };

        return refreshToken;
    }

    public Task<string> GenerateTokenAsync(Account account)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, account.Id),
            new(ClaimTypes.Name, account.Username),
            new(ClaimTypes.Role, account.Role.Name)
        };

       
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = _configuration["JWT:Issuer"],
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}