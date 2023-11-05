using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.LandType;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface ILandTypeRepository : IGetAllAsync<LandType>,
        IFindAsync<LandType>,
        IAddAsync<LandType>,
        IDelete<LandType>
    {
        Task<LandType?> FindByCodeAsync(string code);
        Task<LandType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted);
        Task<LandType?> FindByNameAndIsDeletedStatus(string name, bool isDeleted);
        Task<IEnumerable<LandType>?> GetAllActivedLandTypes();
        Task<IEnumerable<LandType>> QueryAsync(LandTypeQuery query, bool trackChanges = false);
    }
}
