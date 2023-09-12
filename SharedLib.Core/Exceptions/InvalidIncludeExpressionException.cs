namespace SharedLib.Core.Exceptions;

public class InvalidIncludeExpressionException : HandledException
{
    public InvalidIncludeExpressionException(Type declarerType, string propName) : base(400,
        $"{propName} is not a property of {declarerType.Name} or is not an includable property.")
    {
    }
}

public class InvalidIncludeExpressionException<T> : InvalidIncludeExpressionException
{
    public InvalidIncludeExpressionException(string propName) : base(typeof(T), propName)
    {
    }
}