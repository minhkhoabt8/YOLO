using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Plan
{
    /// <summary>
    /// Boi Thuong Ho Tro Plan used to export file 
    /// </summary>
    public class BTHTPlanReadDTO
    {
        public string ProjectName { get; set; }

        public string ProjectLocation {  get; set; }

        public string PlanName { get; set; }

        public string PlanLocation { get; set; }

        public string PlanBasedOn { get; set;}

        //Tong dien tich thu hoi dat - chua co field trong PLan entity
        public decimal TotalLandRecoveryArea { get; set; }

        //tong chu so huu
        public int TotalOwnerSupportCompensation {  get; set; }

        //Dia chi thu hoi dat = Plan Location
        public string LandAcquisitionAddress { get; set; }

        //Boi Thuong Ho Tro Dat
        public decimal TotalPriceLandSupportCompensation { get; set; }

        public decimal TotalPriceHouseSupportCompensation { get; set; }

        public decimal TotalPriceArchitectureSupportCompensation { get; set; }

        public decimal TotalPricePlantSupportCompensation { get; set; }

        public decimal TotalPriceOtherSupportCompensation { get; set; }

        //Tong chi phi phuc vu GPMB
        public decimal TotalGpmbServiceCost {  get; set; }
    }
}
