using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Authentication;

namespace Auth.Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AccountReadDTO> LoginAsync(LoginInputDTO inputDTO);
        Task<AccountReadDTO> FirstTimeResetPasswordAsync(ResetPasswordInputDTO resetDTO);
        Task<LoginOutputDTO> LoginWithRefreshTokenAsync(string? token);
        Task<LoginOutputDTO> LoginWithOtpAsync(string userName, string? code);
        Task<string> ResendOtpAsync(LoginInputDTO input);
    }
}
