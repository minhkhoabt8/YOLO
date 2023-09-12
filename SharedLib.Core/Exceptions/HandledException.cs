namespace SharedLib.Core.Exceptions;

public class HandledException : Exception
{
    public int StatusCode { get; set; }

    public HandledException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}