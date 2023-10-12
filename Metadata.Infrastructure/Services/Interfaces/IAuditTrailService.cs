using Metadata.Infrastructure.DTOs.AuditTrail;
using SharedLib.Infrastructure.DTOs;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IAuditTrailService
    {
        Task<PaginatedResponse<AuditTrailReadDTO>> QueryAuditsAsync(AuditTrailQuery query);
    }
}
