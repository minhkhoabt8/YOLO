using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core.Extensions
{
    public interface IGetFileTemplateDirectory
    {
        string GetExport(string fileName);
        string GetImport(string fileName);
        string GetStoragePath();
    }
}
