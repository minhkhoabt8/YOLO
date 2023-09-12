using System.Reflection;
using SharedLib.Core.Attributes;

namespace SharedLib.Core.Exceptions
{
    public class InvalidOrderExpressionException : HandledException
    {
        public InvalidOrderExpressionException(IEnumerable<string> orderableProperties) : base(400,
            $"Failed to parse OrderBy expression. Orderable properties: {string.Join(", ", orderableProperties)}. Format: PropertyName [ASC|DESC], PropertyName [ASC|DESC], ...")
        {
        }

        public InvalidOrderExpressionException(string expr) : base(400, $"Failed to parse order expression: {expr}")
        {
        }
    }

    public class InvalidOrderExpressionException<T> : InvalidOrderExpressionException
    {
        public InvalidOrderExpressionException() : base(typeof(T).GetProperties()
            .Where(prop => prop.GetCustomAttribute<OrderingPropertyAttribute>() != null).Select(prop => prop.Name))
        {
        }
    }
}