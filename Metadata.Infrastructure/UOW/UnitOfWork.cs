using Metadata.Core.Data;
using Metadata.Infrastructure.DTOs.AuditEntry;
using Metadata.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedLib.Infrastructure.Services.Interfaces;
using System;
using System.Security.AccessControl;

namespace Metadata.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YoloMetadataContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<string, object> _singletonRepositories;
        private readonly IUserContextService _userContextService;


        public UnitOfWork(YoloMetadataContext context, IServiceProvider serviceProvider, IUserContextService userContextService)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _userContextService = userContextService;
            _singletonRepositories = new Dictionary<string, object>();
        }

        public IProjectRepository ProjectRepository => GetSingletonRepository<IProjectRepository>();
        public IDocumentRepository DocumentRepository => GetSingletonRepository<IDocumentRepository>();
        public IProjectDocumentRepository ProjectDocumentRepository => GetSingletonRepository<IProjectDocumentRepository>();
        public IOwnerRepository OwnerRepository => GetSingletonRepository<IOwnerRepository>();
        public IPlanRepository PlanRepository => GetSingletonRepository<IPlanRepository>();

        public Task<int> CommitAsync()
        {
            OnBeforeSaveChanges();
            return _context.SaveChangesAsync();

        }

        private T GetSingletonRepository<T>()
        {
            if (!_singletonRepositories.ContainsKey(typeof(T).Name))
            {
                _singletonRepositories[typeof(T).Name] =
                    _serviceProvider.GetService(typeof(T)) ?? throw new InvalidOperationException();
            }
            return (T)_singletonRepositories[typeof(T).Name];
        }

        private void OnBeforeSaveChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditEntry || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged) continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = _userContextService.AccountID;
                auditEntry.UserName = _userContextService.Username;
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = Core.Enums.AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = Core.Enums.AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = Core.Enums.AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                _context.AuditTrails.Add(auditEntry.ToAudit());
            }
        }
    }
}
