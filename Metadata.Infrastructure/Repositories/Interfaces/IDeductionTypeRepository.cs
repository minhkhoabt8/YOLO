using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.DeductionType;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IDeductionTypeRepository : IGetAllAsync<DeductionType>,
        IFindAsync<DeductionType>,
        IAddAsync<DeductionType>,
        IDelete<DeductionType>
    {
        Task<DeductionType?> FindByCodeAsync(string code);
       
        Task<DeductionType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted);
        Task<DeductionType?> FindByNameAndIsDeletedStatus(string name, bool isDeleted);
        Task<IEnumerable<DeductionType>?> GetActivedDeductionTypes();
        Task<IEnumerable<DeductionType>> QueryAsync(DeductionTypeQuery query, bool trackChanges = false);

        Task<DeductionType?> FindByCodeAndIsDeletedStatusForUpdate(string code, string id, bool isDeleted);

        Task<DeductionType?> FindByNameAndIsDeletedStatusForUpdate(string name, string id, bool isDeleted);
    }
}
