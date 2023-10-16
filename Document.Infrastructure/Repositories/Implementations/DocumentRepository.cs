using Document.Core.Data;
using Document.Infrastructure.Repositories.Interfaces;
using SharedLib.Infrastructure.Repositories.Implementations;


namespace Document.Infrastructure.Repositories.Implementations
{
    public class DocumentRepository : GenericRepository<Document.Core.Entities.Document, YoloDocumentContext>, IDocumentRepository
    {
        public DocumentRepository(YoloDocumentContext context) : base(context)
        {
        }
    }
}
