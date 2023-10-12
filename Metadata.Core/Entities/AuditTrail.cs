using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities
{
    public class AuditTrail
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? UserId { get; set; } = null!;

        public string? UserName { get; set; } = null!;

        public string? Type { get; set; } = null!;

        public string TableName { get; set; } 

        public string OldValue { get; set; }

        public string NewValue { get; set; } 

        public string AffectedColumn { get; set; } 

        public string PrimaryKey { get; set; } 

        public DateTime CreatedDate { get; set; } = DateTime.Now.SetKindUtc();
    }
}
