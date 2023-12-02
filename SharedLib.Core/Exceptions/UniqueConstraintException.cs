namespace SharedLib.Core.Exceptions;

public class UniqueConstraintException : HandledException
{
    public UniqueConstraintException(string message) : base(400, message)
    {
    }

    public UniqueConstraintException(string entityClassName, string propertyName, object duplicateValue) : this(
        /*$"Unique property {propertyName} with value ({duplicateValue}) already existed in another {entityClassName}"*/
        $"Thuộc tính {propertyName} có giá trị ({duplicateValue}) đã tồn tại trong một {entityClassName} khác")
    {
    }
}

public class UniqueConstraintException<T> : UniqueConstraintException
{
    public UniqueConstraintException(string propertyName, object duplicateValue) : base(typeof(T).Name,
        propertyName, duplicateValue)
    {
    }
}