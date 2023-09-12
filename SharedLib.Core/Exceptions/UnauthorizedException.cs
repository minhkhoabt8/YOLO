namespace SharedLib.Core.Exceptions;

public class UnauthorizedException: HandledException
{
    public UnauthorizedException() : base(401, "Unauthorized")
    {
    }
}