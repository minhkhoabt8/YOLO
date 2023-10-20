﻿

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
        public ISupportRepository SupportRepository { get; }
        public IDeductionRepository DeductionRepository { get; }
        public IAssetCompensationRepository AssetCompensationRepository { get; }
        public IAttachFileRepository AttachFileRepository { get; }

        Task<int> CommitAsync();
    }
}
