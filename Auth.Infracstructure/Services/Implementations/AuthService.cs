using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using AutoMapper;

namespace Auth.Infrastructure.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<LoginOutputDTO> LoginAsync(LoginInputDTO inputDTO)
        {
            var account = await _unitOfWork.AccountRepository.LoginAsync(inputDTO);

            if (account == null)
            {
                throw new WrongCredentialsException();
            }
            if (account.IsActive == false)
            {
                throw new AccountNotVerifyException();
            }

            var refreshToken = _tokenService.GenerateRefreshToken(account);

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);

            await _unitOfWork.CommitAsync();

            return new LoginOutputDTO
            {
                UserId = account!.Id,
                FullName = account!.Name,
                UserName = account!.Username,
                Role = account!.Role.Name,
                Token = await _tokenService.GenerateTokenAsync(account),
                //TokenExpires = 60 * 60 * 4, // 4 hours
                TokenExpires = 12000,
                RefreshToken = refreshToken.Token ,
                RefreshTokenExpires = refreshToken.ExpiresIn
            };
        }

        //TODO: Implement LoginWithRefreshTokenAsync
        public async Task<LoginOutputDTO> LoginWithRefreshTokenAsync(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidRefreshTokenException();
            }

            var refreshToken = await _unitOfWork.RefreshTokenRepository.FindByTokenIncludeAccountAsync(token) ??
                               throw new InvalidRefreshTokenException();

            // Refresh token compromised => revoke all tokens in family
            if (refreshToken.IsRevoked)
            {
                // Travel down family chain
                while (refreshToken.ReplacedBy != null)
                {
                    // Descendant token
                    refreshToken =
                        await _unitOfWork.RefreshTokenRepository.FindByTokenAsync(refreshToken.ReplacedBy);
                    refreshToken!.Revoke();
                }

                await _unitOfWork.CommitAsync();
                throw new InvalidRefreshTokenException();
            }

            // Expired token
            if (refreshToken.IsExpired)
            {
                throw new InvalidRefreshTokenException();
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken(refreshToken.Account);

            await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);

            refreshToken.ReplaceWith(newRefreshToken);

            await _unitOfWork.CommitAsync();

            return new LoginOutputDTO
            {
                UserId = refreshToken!.Account.Id,
                FullName = refreshToken!.Account.Name,
                UserName = refreshToken!.Account.Username,
                Role = refreshToken!.Account.Role.Name,
                Token = await _tokenService.GenerateTokenAsync(refreshToken.Account),
                //TokenExpires = 60 * 60 * 4, // 4 hours
                TokenExpires = 12000,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpires = refreshToken.ExpiresIn
            };
        }
    }
}
