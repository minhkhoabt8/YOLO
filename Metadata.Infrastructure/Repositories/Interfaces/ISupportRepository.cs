using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Support;
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
        Task<decimal> CaculateTotalSupportOfOwnerAsync(string ownerId);
        Task<IEnumerable<Support?>> GetAllSupportsOfOwnerAsync(string ownerId);
        Task<IEnumerable<Support>> QueryAsync(SupportQuery query, bool trackChanges = false);
    }
}
