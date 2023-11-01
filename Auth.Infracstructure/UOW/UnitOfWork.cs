using Auth.Core.Data;
using Auth.Infrastructure.Repositories.Interfaces;


namespace Auth.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YoloAuthContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<string, object> _singletonRepositories;

        public UnitOfWork(YoloAuthContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _singletonRepositories = new Dictionary<string, object>();
        }

        public IAccountRepository AccountRepository => GetSingletonRepository<IAccountRepository>();
        public IRoleRepository RoleRepository => GetSingletonRepository<IRoleRepository>();
        public IRefreshTokenRepository RefreshTokenRepository => GetSingletonRepository<IRefreshTokenRepository>();
        public INotificationRepository NotificationRepository => GetSingletonRepository<INotificationRepository>(); 
        
        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        private T GetSingletonRepository<T>()
        {
            if (!_singletonRepositories.ContainsKey(typeof(T).Name))
            {
                _singletonRepositories[typeof(T).Name] =
                    _serviceProvider.GetService(typeof(T)) ?? throw new InvalidOperationException();
            }
            return (T)_singletonRepositories[typeof(T).Name];
        }
    }
}
