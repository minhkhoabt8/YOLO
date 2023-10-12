using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Services.Interfaces;
using SharedLib.Infrastructure.Utils;

namespace SharedLib.Infrastructure.Attributes
{
    public interface IEntityAuditor
    {
        IEnumerable<AuditEntry> AuditEntries(DbContext context, IUserContextService userContextService);
    }
}
