namespace SharedLib.Core.Exceptions;

public class InvalidActionException : HandledException
{
    public InvalidActionException(string? message = null) : base(400, message ?? "Không thể thực hiện hành động này.")
    {
    }
}