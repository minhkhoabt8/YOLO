using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using AutoMapper;
using SharedLib.Core.Exceptions;

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

        public async Task<AccountReadDTO> LoginAsync(LoginInputDTO inputDTO)
        {
            //check account with username and password
            var account = await _unitOfWork.AccountRepository.LoginAsync(inputDTO);

            if (account == null || account.IsDelete)
            {
                throw new WrongCredentialsException();
            }

            account.GernerateOTP();

            //call Send SMS
            await _smsService.SendOtpSmsAsync(account.Phone!, account.Otp!);

            await _smsService.SendOtpEmail(account.Email!, account.Otp!);

            await _unitOfWork.CommitAsync();

            return _mapper.Map<AccountReadDTO>(account);

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
                TokenExpires = 7200,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpires = newRefreshToken.ExpiresIn
            };
        }

        public async Task<LoginOutputDTO> LoginWithOtpAsync(string userName, string? code)
        {
            
            var account = await _unitOfWork.AccountRepository.FindAccountByUsernameAsync(userName);

            if (account == null || account.IsDelete == true)
            {
                throw new WrongCredentialsException();
            }

            if (string.IsNullOrEmpty(code) || !account.IsOtpValid(code))
            {
                throw new InvalidOtpException();
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
                TokenExpires = 7200,
                RefreshToken = refreshToken.Token!,
                RefreshTokenExpires = refreshToken.ExpiresIn
            };

        }


        public async Task<string> ResendOtpAsync(LoginInputDTO input)
        {
            var account = await _unitOfWork.AccountRepository.LoginAsync(input);

            if (account == null || account.IsDelete)
            {
                throw new WrongCredentialsException();
            }

            account.GernerateOTP();

            await _unitOfWork.CommitAsync();

            ////call Send SMS
            await _smsService.SendOtpSmsAsync(account.Phone!, account.Otp!);

            await _smsService.SendOtpEmail(account.Email!, account.Otp!);

            return account.Otp!;
        }

        /// <summary>
        /// First Time Reset Password -  Active account !!!
        /// </summary>
        /// <param name="resetDTO"></param>
        /// <returns></returns>
        /// <exception cref="WrongCredentialsException"></exception>
        public async Task<AccountReadDTO> FirstTimeResetPasswordAsync(ResetPasswordInputDTO resetDTO)
        {
            //1.try log in using old password
            var login = new LoginInputDTO 
            {
                Username = resetDTO.Username,
                Password = resetDTO.OldPassword
            };

            //2.check account exist with username and old password
            var account = await _unitOfWork.AccountRepository.LoginAsync(login);

            if (account == null)
            {
                throw new WrongCredentialsException();
            }

            //1.2check if account is Active -> this using to active account so account aldready active can not use this
            if (account.IsActive)
            {
                throw new InvalidActionException();
            }

            //2.if account exist then re-assign Password
            account.Password = resetDTO.NewPassword;

            //3.set is active of account
            if (!account.IsActive) account.IsActive = true;

            await _unitOfWork.CommitAsync();

            return _mapper.Map<AccountReadDTO>(account);

        }

    }
}
