using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Owner;
using Metadata.Infrastructure.DTOs.Project;
using SharedLib.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Plan
{
    public class PlanReadDTO
    {
        public string PlanId { get; set; } = null!;

        public string? ProjectId { get; set; }

        public string PlanReportInfo { get; set; }

        public string PlanLocation { get; set; }

        public string? PlanPhrase { get; set; }

        public string? PlanDescription { get; set; }

        public string? PlanCreateBase { get; set; }

        public string? PlanApprovedBy { get; set; }

        public string? PlanReportSignal { get; set; }

        public DateTime? PlanReportDate { get; set; }

        public DateTime? PlanCreatedTime { get; set; }

        public DateTime? PlanEndedTime { get; set; }

        public string? PlanCreatedBy { get; set; }

        public string? PlanStatus { get; set; }

        //Tong Chu So Huu Ho Tro Boi Thuong
        public int? TotalOwnerSupportCompensation { get; set; }

        //Tong Kinh Phi Boi Thuong
        public decimal? TotalPriceCompensation { get; set; }

        //Tong King Phi Boi Thuong Dat
        public decimal? TotalPriceLandSupportCompensation { get; set; }

        public decimal? TotalPriceHouseSupportCompensation { get; set; }

        public decimal? TotalPriceArchitectureSupportCompensation { get; set; }

        public decimal? TotalPricePlantSupportCompensation { get; set; }

        public decimal? TotalPriceOtherSupportCompensation { get; set; }

        //Tong Khau Tru
        public decimal? TotalDeduction { get; set; }

        //Tong dien tich thu hoi dat
        public decimal TotalLandRecoveryArea { get; set; }

        public decimal TotalGpmbServiceCost { get; set; } 

        public bool? IsDeleted { get; set; }

        public  ICollection<Core.Entities.AttachFile> AttachFiles { get; set; }

        public  ICollection<OwnerReadDTO> Owners { get; set; }

        
    }
}
