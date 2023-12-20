using Metadata.Core.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using SharedLib.Core.Enums;
using SharedLib.Core.Extensions;

namespace SharedLib.Infrastructure.Utils
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        public AuditTrail ToAudit()
        {
            var audit = new AuditTrail();
            audit.UserId = UserId == null ? "user id is null" : UserId;
            audit.UserName = UserName == null ? "user name is null" : UserName;
            audit.Type = AuditType.ToString();
            audit.TableName = TableName;
            audit.CreatedDate = DateTime.UtcNow.SetKindUtc();
            audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
            audit.OldValue = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValue = NewValues.Count == 0 ? "Completely Deleted" : JsonConvert.SerializeObject(NewValues);
            audit.AffectedColumn = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            return audit;
        }
    }
}
