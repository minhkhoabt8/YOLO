
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedLib.Core.Enums;
using SharedLib.Infrastructure.Services.Interfaces;
using SharedLib.Infrastructure.Utils;

namespace SharedLib.Infrastructure.Attributes
{
    public class EntityAuditor : IEntityAuditor
    {
        public IEnumerable<AuditEntry> AuditEntries(DbContext context, IUserContextService userContextService)
        {
            context.ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (ShouldAuditEntry(entry))
                {
                    var auditEntry = CreateAuditEntry(entry, userContextService);
                    PopulateAuditEntryProperties(auditEntry, entry);
                    auditEntries.Add(auditEntry);
                }
            }

            return auditEntries;
        }

        private bool ShouldAuditEntry(EntityEntry entry)
        {
            return !(entry.Entity is AuditEntry) && entry.State != EntityState.Detached && entry.State != EntityState.Unchanged;
        }

        private AuditEntry CreateAuditEntry(EntityEntry entry, IUserContextService userContextService)
        {
            return new AuditEntry(entry)
            {
                TableName = entry.Entity.GetType().Name,
                UserId = userContextService.AccountID,
                UserName = userContextService.Username
            };
        }
        private void PopulateAuditEntryProperties(AuditEntry auditEntry, EntityEntry entry)
        {
            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;

                if (property.Metadata.IsPrimaryKey())
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = SharedLib.Core.Enums.AuditType.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }
    }
}
