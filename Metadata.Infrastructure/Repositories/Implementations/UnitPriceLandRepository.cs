using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.Repositories.Interfaces;
using SharedLib.Infrastructure.Repositories.Implementations;


namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class UnitPriceLandRepository : GenericRepository<UnitPriceLand, YoloMetadataContext>, IUnitPriceLandRepository
    {
        public UnitPriceLandRepository(YoloMetadataContext context) : base(context)
        {
        }
    }
}
