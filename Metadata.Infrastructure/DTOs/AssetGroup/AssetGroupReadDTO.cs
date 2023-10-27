using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.AssetGroup
{
    public class AssetGroupReadDTO
    {
        public string AssetGroupId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
