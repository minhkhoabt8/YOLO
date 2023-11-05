using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IDeductionRepository :
        IAddAsync<Deduction>,
        IFindAsync<Deduction>,
        IGetAllAsync<Deduction>,
        IUpdate<Deduction>,
        IDelete<Deduction>
    {
        Task<IEnumerable<Deduction?>> GetAllDeductionsOfOwnerAsync(string ownerId);
        Task<decimal> CaculateTotalDeductionOfOwnerAsync(string ownerId);
    }
}
