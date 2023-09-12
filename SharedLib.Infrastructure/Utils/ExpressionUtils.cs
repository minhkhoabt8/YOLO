using System.Linq.Expressions;
using System.Reflection;

namespace SharedLib.Infrastructure.Utils
{
    public static class ExpressionUtils
    {
        public static LambdaExpression ToLowerString()
        {
            var inputStringExpression = Expression.Parameter(typeof(string));

            var toLowerMethodInfo = typeof(String).GetTypeInfo().GetDeclaredMethods(nameof(String.ToLower))
                .Single(m => m.GetParameters().Length == 0);

            var toLowerMethodCallExpression = Expression.Call(inputStringExpression, toLowerMethodInfo);

            return Expression.Lambda<Func<string, string>>(toLowerMethodCallExpression, inputStringExpression);
        }
    }
}