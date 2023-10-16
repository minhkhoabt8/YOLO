using Document.Infrastructure.Repositories.Interfaces;
using Metadata.Core.Data;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Attributes;
using SharedLib.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YoloMetadataContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<string, object> _singletonRepositories;
        private readonly IUserContextService _userContextService;
        private readonly IEntityAuditor _entityAuditor;

        public UnitOfWork(YoloMetadataContext context, IServiceProvider serviceProvider, IDictionary<string, object> singletonRepositories, IUserContextService userContextService, IEntityAuditor entityAuditor)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _singletonRepositories = singletonRepositories;
            _userContextService = userContextService;
            _entityAuditor = entityAuditor;
        }

        public IDocumentRepository DocumentRepository => GetSingletonRepository<IDocumentRepository>();

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
