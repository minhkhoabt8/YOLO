using SharedLib.Infrastructure.Repositories.Interfaces;


namespace Document.Infrastructure.Repositories.Interfaces
{
    public interface IDocumentRepository :
        IFindAsync<Core.Entities.Document>,
        IAddAsync<Core.Entities.Document>,
        IUpdate<Core.Entities.Document>,
        IDelete<Core.Entities.Document>
    {
    }
}
