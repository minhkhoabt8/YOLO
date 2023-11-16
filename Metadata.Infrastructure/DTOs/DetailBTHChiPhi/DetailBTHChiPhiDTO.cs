using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.DetailBTHChiPhi
{
    public class DetailBTHChiPhiDTO
    {

        public string Stt { get; set; } = Guid.NewGuid().ToString();
        public string OwnerCode { get; set; }

        //PriceAppliedCode
        public string UnitPriceCode { get; set; }

        public string ProjectCode { get; set; }
        public string OwnerName { get; set; }

        public string? Province { get; set; }

        public string? District { get; set; }

        public string? Ward { get; set; }

        public string? MeasuredPageNumber { get; set; }

        public string? MeasuredPlotNumber { get; set; }

        public string CodeLandType { get; set; }

        public string? MeasuredPlotArea { get; set; }

        public decimal? WithdrawArea { get; set; }

        public decimal? LandCompensationPrice { get; set; }


        public string AssetType { get; set; }

        public decimal AssetCompensationPrice { get; set; }


        public decimal SupportPrice { get; set; }
        public decimal DeductionPrice { get; set; }

        public decimal SumBTHT { get; set; }



    }
}
