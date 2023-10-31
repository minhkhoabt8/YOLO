using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core.Extensions
{
    public class GetFileTemplateDirectory
    {
        /// <summary>
        /// Get File Template Path Based on FileName
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Get(string fileName)
        {
            string templateDirectory = Path.Combine(Environment.CurrentDirectory, "ReportTemplates");

            switch (fileName)
            {
                case "BangTongHopChiPhiBT":

                    return Path.Combine(templateDirectory, "BangTongHopChiPhiBT.xlsx");

                case "BangTongHopThuHoi":

                    return Path.Combine(templateDirectory, "BangTongHopThuHoi.xlsx");

                case "Bienbanxacdinhgia":

                    return Path.Combine(templateDirectory, "Bienbanxacdinhgia.xlsx");

                case "Danhsachduan":

                    return Path.Combine(templateDirectory, "Danhsachduan.xlsx");

                case "PhuongAn_BaoCao":

                    return Path.Combine(templateDirectory, "PhuongAn_BaoCao.docx");

                default:
                    return "File not found";
            }
        }
    }
}
