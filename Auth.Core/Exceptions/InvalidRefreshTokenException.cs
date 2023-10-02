using SharedLib.Core.Exceptions;

namespace Auth.Core.Exceptions;

public class InvalidRefreshTokenException : HandledException
{
    public InvalidRefreshTokenException() : base(401, "Missing or invalid refresh token")
    {
    }
}