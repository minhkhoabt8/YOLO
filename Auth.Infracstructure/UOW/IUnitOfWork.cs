using Auth.Infrastructure.Repositories.Interfaces;


namespace Auth.Infrastructure.UOW
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        
        Task<int> CommitAsync();
    }
}
