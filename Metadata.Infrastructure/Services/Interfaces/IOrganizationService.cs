using Metadata.Infrastructure.DTOs.OrganizationType;
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
        Task<bool> DeleteAsync(string delete);
    }
}
