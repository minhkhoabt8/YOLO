using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.Core.Attributes
{
    public class InputTypeAttribute : Attribute
    {
        public Type AllowedType { get; }

        public InputTypeAttribute(Type allowedType)
        {
            AllowedType = allowedType;
        }
    }
}
