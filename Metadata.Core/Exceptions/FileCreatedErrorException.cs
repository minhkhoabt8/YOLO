using SharedLib.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core.Exceptions
{
    public class FileCreatedErrorException : HandledException
    {
        public FileCreatedErrorException() : base(403, "Can Not Create File")
        {
        }
    }
}
