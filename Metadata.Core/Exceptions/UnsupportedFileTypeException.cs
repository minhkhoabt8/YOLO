using SharedLib.Core.Exceptions;

namespace Metadata.Core.Exceptions;

public class UnsupportedFileTypeException : HandledException
{
    public UnsupportedFileTypeException() : base(400, "The file type is currently not suppported")
    {
    }
}