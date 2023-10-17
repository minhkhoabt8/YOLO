﻿

using Metadata.Infrastructure.Repositories.Interfaces;
using Metadata.Infrastructure.Services.Interfaces;

namespace Metadata.Infrastructure.UOW
{
    public interface IUnitOfWork
    {
        public IDocumentRepository DocumentRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IProjectDocumentRepository ProjectDocumentRepository { get; }
        public IPlanRepository PlanRepository { get; }
        public IAuditTrailRepository AuditTrailRepository { get; }
        public IOwnerRepository OwnerRepository { get; }
        
        
        public ILandTypeRepository LandTypeRepository { get; }
        public ILandGroupRepository LandGroupRepository { get; }
        public ISupportTypeRepository SupportTypeRepository { get; }
        public IAssetGroupRepository AssetGroupRepository { get; }
        public IOrganizationTypeRepository OrganizationTypeRepository { get; }
        public IDeductionTypeRepository DeductionTypeRepository { get; }

        public IDocumentTypeRepository DocumentTypeRepository { get; }

        public IAssetUnitRepository AssetUnitRepository { get; }

        
        

        
        Task<int> CommitAsync();
    }
}
