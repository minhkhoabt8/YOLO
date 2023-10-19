using Metadata.Core.Entities;
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
        Task<IEnumerable<DeductionType>?> GetAllDeletedDeductionTypes();
        Task<DeductionType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted);
    }
}
