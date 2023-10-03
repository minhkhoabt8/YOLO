using Metadata.Core.Data;
using Metadata.Infrastructure.Repositories.Interfaces;

namespace Metadata.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YoloMetadataContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<string, object> _singletonRepositories;

        public UnitOfWork(YoloMetadataContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _singletonRepositories = new Dictionary<string, object>();
        }

        public IProjectRepository ProjectRepository => GetSingletonRepository<IProjectRepository>();
        public IDocumentRepository DocumentRepository => GetSingletonRepository<IDocumentRepository>();
        public IProjectDocumentRepository ProjectDocumentRepository => GetSingletonRepository<IProjectDocumentRepository>();

        public ILandGroupRepository LandGroupRepository => GetSingletonRepository<ILandGroupRepository>();

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
