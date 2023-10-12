using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities
{
    public class AuditTrail
    {
        [Key]
        public int Id { get; set; }

        public string? EntityName { get; set; }

        public string? Action { get; set; }

        public string? EntityIdentifier { get; set; }

        public string PropertyName { get; set; } = null!;

        public string OldValue { get; set; } = null!;

        public string NewValue { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.Now.SetKindUtc();
    }
}
