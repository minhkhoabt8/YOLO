using Metadata.Core.Entities;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface ILandGroupRepository : IGetAllAsync<LandGroup>,
        IFindAsync<LandGroup>,
        IAddAsync<LandGroup>,
        IDelete<LandGroup>
    {
        Task<LandGroup?> FindByCodeAsync(string code);
       
    }
}
