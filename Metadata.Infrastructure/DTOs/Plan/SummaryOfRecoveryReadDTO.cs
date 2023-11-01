using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.DTOs.Plan
{
    public class SummaryOfRecoveryReadDTO
    {
        //maVanBan = "[" + phuongan.MaChuSoHuu + "]" + "/[" + duan.MaApGia + "]-[" + duan.MaDuAn + "]",
        public string DocumentCode { get; set; }

        public string OwnerName { get; set; }

        public string LandPosition { get; set; }

        public string PlotNumber { get; set; }

        public string PageNumber { get; set; }

        public string LandType { get; set; }

        //Dien tich dat thuc te
        public decimal LandArea { get; set; }

        //Dien tich dat thu hoi
        public decimal WidthdrawArea { get; set; }


    }
}
