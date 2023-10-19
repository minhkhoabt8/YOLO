using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface ISupportTypeRepository : IGetAllAsync<SupportType>,
        IFindAsync<SupportType>,
        IAddAsync<SupportType>,
        IDelete<SupportType>
    {
        Task<SupportType?> FindByCodeAsync(string code);
        Task<SupportType?> FindByCodeAndIsDeletedStatus(string code, bool isDeleted);
        Task<IEnumerable<SupportType>?> GetAllDeletedSupportType();

    }
    
}
