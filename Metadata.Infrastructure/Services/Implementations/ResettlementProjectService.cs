using Metadata.Infrastructure.DTOs.Project;
using Metadata.Infrastructure.DTOs.ResettlementProject;
using Metadata.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class ResettlementProjectService : IResettlementProjectService
    {
        public Task<ResettlementProjectReadDTO> CreateResettlementProjectAsync(ResettlementProjectWriteDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResettlementProjectReadDTO>> CreateResettlementProjectsAsync(IEnumerable<ResettlementProjectWriteDTO> dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteResettlementProjectAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ResettlementProjectReadDTO>> GetAllResettlementProjectsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProjectReadDTO> GetResettlementProjectAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResettlementProjectReadDTO> UpdateResettlementProjectAsync(string id, ResettlementProjectWriteDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
