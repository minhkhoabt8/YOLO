using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.UnitPriceAsset;
using SharedLib.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IPriceAppliedCodeRepository : IGetAllAsync<PriceAppliedCode>,
        IFindAsync<PriceAppliedCode>,
        IAddAsync<PriceAppliedCode>,
        IDelete<PriceAppliedCode>
    {
    }
}
