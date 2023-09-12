using System.Reflection;
using SharedLib.Core.Attributes;
using SharedLib.Core.Exceptions;
using SharedLib.Infrastructure.DTOs;
using System.Linq.Dynamic.Core;

namespace SharedLib.Infrastructure.Repositories.QueryExtensions
{
    public static class OrderedQueryExtensions
    {
        public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string order)
        {
            var expression = ParseOrder<T>(order);

            return query.OrderBy(expression);
        }

        private static string ParseOrder<T>(string order)
        {
            var orderingProperties = typeof(T).GetProperties()
                .Where(prop => prop.GetCustomAttribute<OrderingPropertyAttribute>() != null)
                .Select(prop => prop.Name.ToLower()).ToHashSet();

            var parsedExpressions = new List<string>();

            foreach (var exp in order.Split(',', StringSplitOptions.TrimEntries))
            {
                var elements = exp.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Not an ordering property or badly formated order expression
                if (elements.Length != 2 ||
                    !orderingProperties.Contains(elements[0].ToLower()) ||
                    elements[1].ToUpper() != IOrderedQuery.ASCENDING_EXPRESSION &&
                    elements[1].ToUpper() != IOrderedQuery.DESCENDING_EXPRESSION)
                {
                    throw new InvalidOrderExpressionException<T>();
                }

                var property = elements[0];
                var isAscending = elements[1].ToUpper() == IOrderedQuery.ASCENDING_EXPRESSION;

                parsedExpressions.Add(
                    $"{property} {(isAscending ? IOrderedQuery.ASCENDING_EXPRESSION : IOrderedQuery.DESCENDING_EXPRESSION)}");
            }

            return string.Join(", ", parsedExpressions);
        }
    }
}