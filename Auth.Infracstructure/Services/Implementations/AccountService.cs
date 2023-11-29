using Auth.Core.Entities;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using AutoMapper;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Data;

namespace Auth.Infrastructure.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISmsService _smsService;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, ISmsService smsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _smsService = smsService;
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

            await _unitOfWork.AccountRepository.AddAsync(newAccount);

            //call Send SMS
            await _smsService.SendPasswordSmsAsync(newAccount.Phone!, newAccount.Password!);

            await _smsService.SendPasswordEmail(newAccount.Email!, newAccount.Password!);

            await _unitOfWork.CommitAsync();

            newAccount.Role = await _unitOfWork.RoleRepository.FindAsync(writeDTO.RoleId);

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

        

        public async Task<AccountReadDTO> UpdateAccountAsync(string Id, AccountWriteDTO accountReadDTO)
        {
            var account = await _unitOfWork.AccountRepository.FindAsync(Id);
            if (account == null) throw new EntityWithIDNotFoundException<Account>(Id);
            _mapper.Map(accountReadDTO, account);
            return _mapper.Map<AccountReadDTO>(account);
        }

        public async Task DeleteAccountAsync(string id)
        {
            var account = await _unitOfWork.AccountRepository.FindAsync(id);
            if (account == null) throw new EntityWithIDNotFoundException<Account>(id);
            account.IsDelete = true;
            await _unitOfWork.CommitAsync();
        }

        public async Task<AccountReadDTO> GetAccountByIdAsync(string id)
        {
            return _mapper.Map<AccountReadDTO>(await _unitOfWork.AccountRepository.FindAsync(id, include: "Role"));
        }

        public async Task<PaginatedResponse<AccountReadDTO>> QueryAccount(AccountQuery query)
        {
            var accounts = await _unitOfWork.AccountRepository.QueryAsync(query);

            return PaginatedResponse<AccountReadDTO>.FromEnumerableWithMapping(accounts, query, _mapper);
        }
    }
}
