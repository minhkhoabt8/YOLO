using Metadata.Infrastructure.DTOs.LandType;
using Metadata.Infrastructure.DTOs.OrganizationType;
using SharedLib.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IOrganizationService
    {   
        Task<IEnumerable<OrganizationTypeReadDTO>> GetAllOrganizationTypeAsync();
        Task<OrganizationTypeReadDTO?> GetAsync(string code);
        Task<OrganizationTypeReadDTO?> CreateOrganizationTypeAsync(OrganizationTypeWriteDTO organizationTypeWriteDTO);
        Task<OrganizationTypeReadDTO?> UpdateAsync(string id, OrganizationTypeWriteDTO organizationTypeUpdateDTO);
        Task<IEnumerable<OrganizationTypeReadDTO>> GetAllActivedOrganizationTypeAsync();
        Task<bool> DeleteAsync(string delete);
        Task<IEnumerable<OrganizationTypeReadDTO>> CreateOrganizationTypesAsync(IEnumerable<OrganizationTypeWriteDTO> organizationTypeWriteDTOs);
        Task CheckNameOrganizationTypeNotDuplicate(string name);
        Task CheckCodeOrganizationTypeNotDuplicate(string code);
        Task<PaginatedResponse<OrganizationTypeReadDTO>> QueryOrganizationTypeAsync(OrganizationTypeQuery paginationQuery);
        Task ImportOrganizationTypeFromExcelAsync(string filePath);
    }
}
