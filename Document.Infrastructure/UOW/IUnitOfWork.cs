using Document.Infrastructure.Repositories.Interfaces;


namespace Document.Infrastructure.UOW
{
    public interface IUnitOfWork
    {
        public IDocumentRepository DocumentRepository { get; }
        Task<int> CommitAsync();
    }
}
