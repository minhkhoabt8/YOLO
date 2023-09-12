using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedLib.Infrastructure.Services.Interfaces;

namespace SharedLib.Infrastructure.Services.Implementations;

public class JWTTokenService : ITokenService
{
    public static readonly Guid SYSTEM_ACCOUNT_ID = new("f1eaca5e-fad5-1eaf-fa11-babb1ed0b0e5");
    private readonly IConfiguration _configuration;

    public JWTTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(params Claim[] claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));


        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenClaims = claims.ToList();

        // Add default system account id
        if (tokenClaims.All(cl => cl.Type != ClaimTypes.NameIdentifier))
        {
            tokenClaims.Add(new Claim(ClaimTypes.NameIdentifier, SYSTEM_ACCOUNT_ID.ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(tokenClaims),
            Issuer = _configuration["JWT:Issuer"],
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}