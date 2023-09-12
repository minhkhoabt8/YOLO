namespace SharedLib.Core.Exceptions;

public class ForbiddenException : HandledException
{
    public ForbiddenException() : base(403, "Forbidden")
    {
    }

    public ForbiddenException(string resourceName) : base(403,
        $"User is not authorized to access this {resourceName}")
    {
    }
}

public class ForbiddenException<T> : ForbiddenException
{
    public ForbiddenException() : base(typeof(T).Name)
    {
    }
}