using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Plan
{
    public class DetailBTHChiPhiReadDTO
    {

        public int Stt { get; set; } 

        public string OwnerCode { get; set; }

        //Project
        public string PriceAppliedCodeId { get; set; }

        //project 
        public string ProjectCode { get; set; }

        //owner
        public string OwnerName { get; set; }

        //project
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }

        //measured landinfor
        public string? MeasuredPageNumber { get; set; }
        public string? MeasuredPlotNumber { get; set; }
        public string? MeasuredPlotArea { get; set; }
        public decimal? WithdrawArea { get; set; }

        //Code in landtype
        public string CodeLandType { get; set; }


        //mesured landinfo
        public decimal? SumLandCompensation { get; set; }

        //AssetCompensation
        public decimal SumHouseCompensationPrice { get; set; }
        public decimal SumTreeCompensationPrice { get; set; }
        public decimal SumArchitectureCompensationPrice { get; set; }


        //Support
        public decimal SumSupportPrice { get; set; }
        //Deduction
        public decimal SumDeductionPrice { get; set; }


        public decimal SumBTHT { get; set; }



    }
}
