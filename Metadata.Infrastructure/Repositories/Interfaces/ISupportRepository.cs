using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface ISupportRepository :
         IGetAllAsync<Support>,
         IFindAsync<Support>,
         IAddAsync<Support>,
         IUpdate<Support>,
         IDelete<Support>
    {
        Task<IEnumerable<Support?>> GetAllSupportsOfOwnerAsync(string ownerId);
    }
}
