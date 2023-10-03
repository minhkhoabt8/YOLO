

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

        Task<int> CommitAsync();
    }
}
