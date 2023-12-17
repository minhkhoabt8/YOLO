using Metadata.Infrastructure.DTOs.LandResettlement;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface ILandResettlementService
    {
        Task<LandResettlementReadDTO> CreateLandResettlementAsync(LandResettlementWriteDTO dto);
        Task<IEnumerable<LandResettlementReadDTO>> CreateLandResettlementsAsync(IEnumerable<LandResettlementWriteDTO> dto);
        Task<IEnumerable<LandResettlementReadDTO>> GetAllLandResettlementsAsync();
        Task<LandResettlementReadDTO> UpdateLandResettlementAsync(string id, LandResettlementWriteDTO dto);
        Task DeleteLandResettlementAsync(string id);
        Task<LandResettlementReadDTO> GetLandResettlementAsync(string id);
        Task<IEnumerable<LandResettlementReadDTO>> GetLandResettlementsOfOwnerAsync(string ownerId);
        Task<IEnumerable<LandResettlementReadDTO>> GetLandResettlementsOfResettlementProjectAsync(string resettlementProjectId);
        Task<decimal> CalculateOwnerTotalLandResettlementPriceInPlanAsync(string planId);

        Task<LandResettlementReadDTO> CheckDuplicateLandResettlementAsync(string pageNumber, string plotNumber);
    }
}
