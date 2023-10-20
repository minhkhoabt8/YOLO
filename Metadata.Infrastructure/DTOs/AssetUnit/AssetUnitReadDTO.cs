using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.AssetUnit
{
    public class AssetUnitReadDTO
    {
        public string AssetUnitId { get; set; } 

        public string Code { get; set; } 

        public string Name { get; set; } 

        public bool? IsDeleted { get; set; } 
    }
}
