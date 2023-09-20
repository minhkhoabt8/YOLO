using Auth.Infrastructure.DTOs.Authentication;

namespace Auth.Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginOutputDTO> LoginAsync(LoginInputDTO inputDTO);
        Task<LoginOutputDTO> LoginWithRefreshTokenAsync(string? token);
    }
}
