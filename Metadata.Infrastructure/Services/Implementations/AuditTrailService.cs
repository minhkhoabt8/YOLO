using AutoMapper;
using Metadata.Infrastructure.DTOs.AuditTrail;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using SharedLib.Infrastructure.DTOs;


namespace Metadata.Infrastructure.Services.Implementations
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditTrailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<AuditTrailReadDTO>> QueryAuditsAsync(AuditTrailQuery query)
        {
            var audits = await _unitOfWork.AuditTrailRepository.QueryAsync(query);

            return PaginatedResponse<AuditTrailReadDTO>.FromEnumerableWithMapping(audits, query, _mapper); ;
        }

    }
}
