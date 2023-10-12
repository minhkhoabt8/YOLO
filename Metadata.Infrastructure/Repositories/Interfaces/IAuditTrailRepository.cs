using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.AuditTrail;
using SharedLib.Infrastructure.Repositories.Interfaces;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface IAuditTrailRepository :
        IFindAsync<AuditTrail>,
        IQueryAsync<AuditTrail, AuditTrailQuery>
    {
    }
}
