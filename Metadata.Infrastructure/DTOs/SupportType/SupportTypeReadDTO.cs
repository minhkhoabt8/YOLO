﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.SupportType
{
    public class SupportTypeReadDTO
    {
        public string SupportTypeId { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public bool? IsDeleted { get; set; } = false;
    }
}
