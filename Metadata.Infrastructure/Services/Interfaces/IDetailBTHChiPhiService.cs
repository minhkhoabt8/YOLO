using Metadata.Infrastructure.DTOs.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Interfaces
{
    public interface IDetailBTHChiPhiService
    {

        Task<IEnumerable<DetailBTHChiPhiReadDTO>> GetAll();


    }
}
