﻿using SharedLib.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Core.Exceptions
{
    public class CanNotAssignUserException : HandledException
    {
        /// <summary>
        /// if user token not have username
        /// </summary>
        public CanNotAssignUserException() : base(403, "Can Not Assign User")
        {
        }

    }
}