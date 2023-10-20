using Metadata.Core.Data;
using Metadata.Core.Entities;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class AssetCompensationRepository : GenericRepository<AssetCompensation, YoloMetadataContext>, IAssetCompensationRepository
    {
        public AssetCompensationRepository(YoloMetadataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AssetCompensation?>> GetAllAssetCompensationsOfOwnerAsync(string ownerId)
        {
            return await _context.AssetCompensations.Include(c=>c.AttachFiles).Where(c => c.OwnerId == ownerId).ToListAsync();
        }
    }
}
