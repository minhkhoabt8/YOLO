using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.LandResettlement;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface ILandResettlementRepository : 
        IAddAsync<LandResettlement>,
        IFindAsync<LandResettlement>,
        IGetAllAsync<LandResettlement>,
        IDelete<LandResettlement>,
        IUpdate<LandResettlement>
    {
        Task<IEnumerable<LandResettlement>> GetLandResettlementsOfOwnerIncludeResettlementProjectAsync(string ownerId);
        Task<IEnumerable<LandResettlement>> GetLandResettlementsOfResettlementProjectIncludeOwnerAsync(string resettlementProjectId);
        Task<LandResettlement?> CheckDuplicateLandResettlement(string pageNumber, string plotNumber);

    }
}
