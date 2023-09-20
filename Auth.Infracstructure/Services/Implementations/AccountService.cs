using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Role;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using AutoMapper;
using SharedLib.Core.Exceptions;
using System;

namespace Auth.Infrastructure.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync()
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync(include: "Role");

            return _mapper.Map<IEnumerable<AccountReadDTO>>(accounts);
        }
        
        public async Task<AccountReadDTO> CreateAccountAsync(AccountWriteDTO writeDTO)
        {

            //check account username exist
            var existAccount = await _unitOfWork.AccountRepository.FindAccountByUsernameAsync(writeDTO.Username);

            if (existAccount != null)
            {
                throw new UniqueConstraintException<Account>(nameof(Account.Username), writeDTO.Username);
            }

            var newAccount = _mapper.Map<Account>(writeDTO);

            newAccount.Name = newAccount.Username;

            newAccount.Password = GeneratePassword();

            // generate otp 

            newAccount.Otp = GenerateOtp();

            await _unitOfWork.AccountRepository.AddAsync(newAccount);

            //TODO: call Send SMS


            await _unitOfWork.CommitAsync();

            return _mapper.Map<AccountReadDTO>(newAccount);
        }



        private string GeneratePassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

            char[] password = new char[12];

            Random random = new Random();

            for (int i = 0; i < 12; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
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
