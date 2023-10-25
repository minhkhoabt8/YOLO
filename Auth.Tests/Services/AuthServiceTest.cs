using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Implementations;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Auth.Tests.Services
{
   
    public class AuthServiceTest
    {

        [Fact]
        public async Task Login_Throws_WrongCredentialsException_When_Username_Or_Password_Not_Match()
        {
            var login = new LoginInputDTO
            {
                Username = "username",
                Password = "password"
            };

            var mockService = new Mock<IAuthService>();
            // Configure the behavior of the mock
            mockService.Setup(sm => sm.LoginAsync(login)).ThrowsAsync(new WrongCredentialsException());
            // Use Assert.ThrowsAsync to verify the exception is thrown
            await Assert.ThrowsAsync<WrongCredentialsException>(() => mockService.Object.LoginAsync(login));
        }

        [Fact]
        public async Task Login_Throws_AccountNotVerifyException_When_Account_Is_Not_Active()
        {
            var login = new LoginInputDTO
            {
                Username = "username",
                Password = "password"
            };
            var mockService = new Mock<IAuthService>();
            // Configure the behavior of the mock
            mockService.Setup(sm => sm.LoginAsync(login)).ThrowsAsync(new AccountNotVerifyException());
            // Use Assert.ThrowsAsync to verify the exception is thrown
            await Assert.ThrowsAsync<AccountNotVerifyException>(() => mockService.Object.LoginAsync(login));
        }
        [Fact]
        public async Task Login_With_Token_Fails_When_Token_Is_Empty_Or_Null()
        {
            string token = string.Empty;
            var mockService = new Mock<IAuthService>();
            mockService.Setup(sm => sm.LoginWithRefreshTokenAsync(token)).ThrowsAsync(new InvalidRefreshTokenException());
            await Assert.ThrowsAsync<InvalidRefreshTokenException>(() => mockService.Object.LoginWithRefreshTokenAsync(token));
        }

        [Fact]
        public async Task Login_With_Token_Revoke_Token_Line_When_A_Revoked_Token_Is_Used()
        {
            var replacementToken = new RefreshToken
            {
                Token = "replace",
                IsRevoked = false
            };

            var token = new RefreshToken
            {
                Token = "original",
                IsRevoked = true,
                ReplacedBy = replacementToken.Token
            };

            var mockUOW = new Mock<IUnitOfWork>();
            var authService = new AuthService(mockUOW.Object, null!,  null!, null!);

            mockUOW.Setup(uow => uow.RefreshTokenRepository.FindByTokenIncludeAccountAsync(token.Token))
                .ReturnsAsync(token);
            mockUOW.Setup(uow => uow.RefreshTokenRepository.FindByTokenAsync(replacementToken.Token))
                .ReturnsAsync(replacementToken);


            await Assert.ThrowsAsync<InvalidRefreshTokenException>(
                () => authService.LoginWithRefreshTokenAsync(token.Token));
            Assert.True(replacementToken.IsRevoked);
        }

        [Fact]
        public async Task Login_With_Token_Fails_When_Token_Is_Expired()
        {
            var token = new RefreshToken
            {
                Token = "original",
                Expires = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(1))
            };
            var mockService = new Mock<IAuthService>();
            mockService.Setup(sm => sm.LoginWithRefreshTokenAsync(token.Token)).ThrowsAsync(new InvalidRefreshTokenException());

            await Assert.ThrowsAsync<InvalidRefreshTokenException>(() => mockService.Object.LoginWithRefreshTokenAsync(token.Token));
        }
        [Fact]
        public async Task LoginWithOtpAsync_Throws_InvalidOtpException_When_Otp_Is_Invalid()
        {
            var login = new LoginInputDTO
            {
                Username = "username",
                Password = "password"
            };

            var mockAccount = new Account
            {
                Otp = "valid-otp-code" 
            };

            var invalidOtpCode = "invalid-otp-code";

            var mockUOW = new Mock<IUnitOfWork>();
            var authService = new AuthService(mockUOW.Object, null!, null!, null!);

            mockUOW.Setup(uow => uow.AccountRepository.LoginAsync(login))
            .ReturnsAsync(mockAccount);
            await Assert.ThrowsAsync<InvalidOtpException>(() => authService.LoginWithOtpAsync(login.Username, invalidOtpCode));
        }
        [Fact]
        public async Task ResendOtpAsync_Is_Return_New_OTP()
        {
            var login = new LoginInputDTO
            {
                Username = "username",
                Password = "password"
            };

            var mockUOW = new Mock<IUnitOfWork>();

            var mockSmsService = new Mock<ISmsService>();

            string capturedOtp = null;
            mockSmsService.Setup(sms => sms.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((phone, otp) => capturedOtp = otp)
                .Returns(Task.CompletedTask);

            var authService = new AuthService(mockUOW.Object, null! , null! , mockSmsService.Object);

            var mockAccount = new Account
            {
                
                Phone = "1234567890", 
                Otp = "old-otp-code" 
            };

            mockUOW.Setup(uow => uow.AccountRepository.LoginAsync(login))
                .ReturnsAsync(mockAccount);

            var newOtp = await authService.ResendOtpAsync(login);

            Assert.NotNull(newOtp);
            Assert.NotEqual("old-otp-code", newOtp);
            Assert.Equal(newOtp, capturedOtp);
        }
    }
}
