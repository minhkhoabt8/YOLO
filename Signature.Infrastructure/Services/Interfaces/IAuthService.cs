using Signature.Infrastructure.DTOs.AccountMapping;

namespace Signature.Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AccountMappping> GetAccountByIdAsync(string Id);
    }
}
