using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.AuditTrail
{
    public class AuditTrailWriteDTO
    {
        public string? UserId { get; set; } = null!;

        public string? UserName { get; set; } = null!;

        public string? Type { get; set; } = null!;

        public string? TableName { get; set; } = "";

        public string? OldValue { get; set; }

        public string? NewValue { get; set; } = "";

        public string? AffectedColumn { get; set; }

        public string? PrimaryKey { get; set; } = "";
    }
}
