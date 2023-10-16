

using Metadata.Infrastructure.Repositories.Interfaces;

namespace Metadata.Infrastructure.UOW
{
    public interface IUnitOfWork
    {
        public IDocumentRepository DocumentRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IProjectDocumentRepository ProjectDocumentRepository { get; }
        public IOwnerRepository OwnerRepository { get; }
        public IPlanRepository PlanRepository { get; }
        public IMeasuredLandInfoRepository MeasuredLandInfoRepository { get; }
        public IAuditTrailRepository AuditTrailRepository { get; }
        public IGCNLandInfoRepository GCNLandInfoRepository { get; }

        Task<int> CommitAsync();
    }
}
