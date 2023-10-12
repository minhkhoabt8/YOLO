using Metadata.Core.Entities;
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
    }
}
