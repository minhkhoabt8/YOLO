using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.AuditTrail
{
    public class AuditTrailReadDTO
    {
        public string Id { get; set; } 

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Type { get; set; }

        public string TableName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string AffectedColumn { get; set; }

        public string PrimaryKey { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
