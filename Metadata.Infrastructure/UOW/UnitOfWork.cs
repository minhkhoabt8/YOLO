using Metadata.Core.Data;
using Metadata.Infrastructure.Repositories.Interfaces;
using SharedLib.Infrastructure.Attributes;
using SharedLib.Infrastructure.Services.Interfaces;


namespace Metadata.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YoloMetadataContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<string, object> _singletonRepositories;
        private readonly IUserContextService _userContextService;
        private readonly IEntityAuditor _entityAuditor;


        public UnitOfWork(YoloMetadataContext context, IServiceProvider serviceProvider, IUserContextService userContextService, IEntityAuditor entityAuditor)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _userContextService = userContextService;
            _entityAuditor = entityAuditor;
            _singletonRepositories = new Dictionary<string, object>();
        }

        public IProjectRepository ProjectRepository => GetSingletonRepository<IProjectRepository>();
        public IDocumentRepository DocumentRepository => GetSingletonRepository<IDocumentRepository>();
        public IProjectDocumentRepository ProjectDocumentRepository => GetSingletonRepository<IProjectDocumentRepository>();
        public IOwnerRepository OwnerRepository => GetSingletonRepository<IOwnerRepository>();
        public IPlanRepository PlanRepository => GetSingletonRepository<IPlanRepository>();
        public IAuditTrailRepository AuditTrailRepository => GetSingletonRepository<IAuditTrailRepository>();
        public IMeasuredLandInfoRepository MeasuredLandInfoRepository => GetSingletonRepository<IMeasuredLandInfoRepository>();
        public IGCNLandInfoRepository GCNLandInfoRepository => GetSingletonRepository<IGCNLandInfoRepository>();
        public ISupportRepository SupportRepository => GetSingletonRepository<ISupportRepository>();
        public IDeductionRepository DeductionRepository => GetSingletonRepository<IDeductionRepository>();
        public IAssetCompensationRepository AssetCompensationRepository => GetSingletonRepository<IAssetCompensationRepository>();

        public IAttachFileRepository AttachFileRepository => GetSingletonRepository<IAttachFileRepository>();

        public ILandGroupRepository LandGroupRepository => GetSingletonRepository<ILandGroupRepository>();

        public ILandTypeRepository LandTypeRepository => GetSingletonRepository<ILandTypeRepository>();

        public ISupportTypeRepository SupportTypeRepository => GetSingletonRepository<ISupportTypeRepository>();

        public IAssetGroupRepository AssetGroupRepository => GetSingletonRepository<IAssetGroupRepository>();

        public IOrganizationTypeRepository OrganizationTypeRepository => GetSingletonRepository<IOrganizationTypeRepository>();

        public IDeductionTypeRepository DeductionTypeRepository => GetSingletonRepository<IDeductionTypeRepository>();

        public IDocumentTypeRepository DocumentTypeRepository => GetSingletonRepository<IDocumentTypeRepository>();

        public IAssetUnitRepository AssetUnitRepository => GetSingletonRepository<IAssetUnitRepository>();
        public Task<int> CommitAsync()
        {
            OnBeforeSaveChanges();
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

        private void OnBeforeSaveChanges()
        {
            var auditEntries = _entityAuditor.AuditEntries(_context, _userContextService);

            foreach (var auditEntry in auditEntries)
            {
                _context.AuditTrails.Add(auditEntry.ToAudit());
            }
        }

    }
}
