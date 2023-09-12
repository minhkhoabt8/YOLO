using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SharedLib.Infrastructure.Attributes
{
    public class UniqueAttribute : ValidationAttribute
    {
        public Type DbContextType { get; set; }
        public Type EntityType { get; set; }
        public string? PropertyName { get; set; }
        public virtual IEnumerable<LambdaExpression> SourceSelectors { get; set; } = Array.Empty<LambdaExpression>();
        public virtual IEnumerable<LambdaExpression> ValueSelectors { get; set; } = Array.Empty<LambdaExpression>();

        public UniqueAttribute(Type dbContextType, Type entityType, string? propertyName = null)
        {
            DbContextType = dbContextType;
            EntityType = entityType;
            PropertyName = propertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            Console.WriteLine(validationContext.MemberName);

            var context = validationContext.GetRequiredService(DbContextType);

            var dbSet = GetSetMethod().Invoke(context, null);

            var findResult = GetFirstOrDefaultMethodInfo().Invoke(null,
                new[] {dbSet, GenerateEqualsExpression(PropertyName ?? validationContext.MemberName!, value)});

            var result = findResult != null || !ValidateBatchState(value, validationContext)
                ? new ValidationResult($"{validationContext.MemberName} already exists")
                : ValidationResult.Success;

            SaveBatchState(value, validationContext);

            return result;
        }


        private MethodInfo GetSetMethod()
        {
            return typeof(DbContext).GetTypeInfo().GetDeclaredMethods(nameof(DbContext.Set))
                .Single(m => m.Name == "Set" && m.GetParameters().Length == 0 && m.GetGenericArguments().Length == 1)
                .MakeGenericMethod(EntityType);
        }

        private MethodInfo GetFirstOrDefaultMethodInfo()
        {
            return typeof(Queryable).GetTypeInfo().GetDeclaredMethods(nameof(Queryable.FirstOrDefault)).Single(m =>
                    m.GetParameters().Length == 2 && m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Last().ParameterType.IsAssignableTo(typeof(Expression)))
                .MakeGenericMethod(EntityType);
        }

        private object GenerateEqualsExpression(string propName, object compareValue)
        {
            var propInfo =
                EntityType.GetProperty(propName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)!;

            var objParameterExpr = Expression.Parameter(EntityType);
            Expression sourceExpression = Expression.Property(objParameterExpr, propInfo);

            foreach (var sourceSelector in SourceSelectors)
            {
                sourceExpression = Expression.Invoke(sourceSelector, sourceExpression);
            }

            Expression valueExpression = Expression.Constant(compareValue);

            foreach (var valueSelector in ValueSelectors)
            {
                valueExpression = Expression.Invoke(valueSelector, valueExpression);
            }

            var equalExpr = Expression.Equal(sourceExpression, valueExpression);
            return Expression.Lambda(typeof(Func<,>).MakeGenericType(EntityType, typeof(bool)), equalExpr,
                objParameterExpr);
        }

        protected virtual void SaveBatchState(object value, ValidationContext context)
        {
            if (!context.Items.ContainsKey($"{nameof(UniqueAttribute)}-{context.MemberName}"))
            {
                Console.WriteLine("No key found");
                context.Items[$"{nameof(UniqueAttribute)}-{context.MemberName}"] = new HashSet<object>();
            }

            ((HashSet<object>) context.Items[$"{nameof(UniqueAttribute)}-{context.MemberName}"]!).Add(value);
        }

        protected virtual bool ValidateBatchState(object value, ValidationContext context)
        {
            return !context.Items.TryGetValue($"{nameof(UniqueAttribute)}-{context.MemberName}", out var values) ||
                   !((HashSet<object>) values!).Contains(value);
        }
    }
}