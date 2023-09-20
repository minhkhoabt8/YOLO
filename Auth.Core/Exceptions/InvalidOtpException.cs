using SharedLib.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Exceptions
{
    public class InvalidOtpException : HandledException
    {
        public InvalidOtpException() : base(403, "Missing or invalid otp code")
        {
        }
    }
}
