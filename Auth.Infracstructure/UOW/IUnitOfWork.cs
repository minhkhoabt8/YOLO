using Auth.Infrastructure.Repositories.Interfaces;


namespace Auth.Infrastructure.UOW
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        INotificationRepository NotificationRepository { get; }
        
        Task<int> CommitAsync();
    }
}
