using SharedLib.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core.Exceptions
{
    public class FileInputException : HandledException
    {
        public FileInputException(string message) : base(400, message)
        {
        }

    }

    public class EntityInputExcelException<T> : FileInputException
    {
        public EntityInputExcelException(string attribute, string value, int row) : base(
            $"{typeof(T).Name} với thuộc tính ({attribute}) có giá trị ({value}) ở cột ({row}) không hợp lệ.")
        {
        }
    }
}
