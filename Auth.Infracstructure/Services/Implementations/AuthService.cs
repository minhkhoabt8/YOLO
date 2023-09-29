using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Dynamic.Core.Tokenizer;

namespace Auth.Infrastructure.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ISmsService _smsService;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, ISmsService smsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _smsService = smsService;
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
                TokenExpires = 12000,
                RefreshToken = refreshToken.Token ,
                RefreshTokenExpires = refreshToken.ExpiresIn
            };
        }

       
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
                TokenExpires = 12000,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpires = newRefreshToken.ExpiresIn
            };
        }

        public async Task<LoginOutputDTO> LoginWithOtpAsync(LoginInputDTO input, string? code)
        {
            
            var account = await _unitOfWork.AccountRepository.LoginAsync(input);

            if (account == null)
            {
                throw new WrongCredentialsException();
            }

            if (string.IsNullOrEmpty(code) || account.Otp != code || account.OtpExpiredAt < DateTime.Now)
            {
                throw new InvalidOtpException();
            }

            account.IsActive = true;

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
                TokenExpires = 12000,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpires = refreshToken.ExpiresIn
            };

        }


        public async Task<string> ResendOtpAsync(LoginInputDTO input)
        {
            var account = await _unitOfWork.AccountRepository.LoginAsync(input);

            if (account == null)
            {
                throw new WrongCredentialsException();
            }

            account.Otp = GenerateOtp();

            ////call Send SMS
            await _smsService.SendSmsAsync(account.Phone, account.Otp);

            await _unitOfWork.CommitAsync();

            return account.Otp;
        }


        private string GenerateOtp()
        {
            const string chars = "0123456789";

            char[] password = new char[6];

            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }
    }
}
