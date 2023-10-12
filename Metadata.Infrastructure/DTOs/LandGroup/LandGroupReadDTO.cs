using Metadata.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.LandGroup
{
    public class LandGroupReadDTO
    {
        public string LandGroupId { get; set; } = null!;

        public string Code { get; set; }

        public string Name { get; set; }

        public bool? IsDeleted { get; set; }

    }
}
