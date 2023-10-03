

using Metadata.Infrastructure.Repositories.Interfaces;

namespace Metadata.Infrastructure.UOW
{
    public interface IUnitOfWork
    {
        public IDocumentRepository DocumentRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IProjectDocumentRepository ProjectDocumentRepository { get; }
        ILandGroupRepository LandGroupRepository { get; }
        Task<int> CommitAsync();
    }
}
