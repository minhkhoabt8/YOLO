using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core.Extensions
{
    public class GetFileTemplateDirectory : IGetFileTemplateDirectory
    {
        /// <summary>
        /// Get File Template Path Based on FileName
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// 
        private readonly string _storagePath;

        public GetFileTemplateDirectory(IConfiguration configuration)
        {
            _storagePath = configuration["StoragePath"]!;
        }
        /// <summary>
        /// Get File Export Storage Path Based On Name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetExport(string fileName)
        {
            string templateDirectory = Path.Combine(_storagePath, "ReportTemplates");

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
                case "BangNhapChuSoHuu":

                    return Path.Combine(templateDirectory, "BangNhapChuSoHuu.xlsx");

                default:
                    return "File not found";
            }
        }

        /// <summary>
        /// Get Machine Configured Storage Path
        /// </summary>
        /// <returns></returns>
        public string GetStoragePath()
        {
            return Path.Combine(_storagePath);
        }

        /// <summary>
        /// Get File Import Storage Path Based On Name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetImport(string fileName)
        {
            string templateDirectory = Path.Combine(_storagePath, "ImportCauHinhTemplates");

            switch (fileName)
            {
                case "OwnerImportTemplate":

                    return Path.Combine(templateDirectory, "OwnerImportTemplate.xlsx");

                case "AssetGroupTemplate":

                    return Path.Combine(templateDirectory, "AssetGroupTemplate.xlsx");

                case "AssetUnitTemplate":

                    return Path.Combine(templateDirectory, "AssetUnitTemplate.xlsx");

                case "DeductionTypeTemplate":

                    return Path.Combine(templateDirectory, "DeductionTypeTemplate.xlsx");

                case "DocumentType":

                    return Path.Combine(templateDirectory, "DocumentType.xlsx");

                case "LandGroupTemplate":

                    return Path.Combine(templateDirectory, "LandGroupTemplate.xlsx");
                case "LandTypeTemplate":

                    return Path.Combine(templateDirectory, "LandTypeTemplate.xlsx");
                case "OrganizationTypeTemplate":

                    return Path.Combine(templateDirectory, "OrganizationTypeTemplate.xlsx");
                case "SupportTypeTemplate":

                    return Path.Combine(templateDirectory, "SupportTypeTemplate.xlsx");

                default:
                    return "File not found";
            }
        }

    }
}