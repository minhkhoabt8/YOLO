using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using SharedLib.Infrastructure.Utils;

namespace SharedLib.Infrastructure.Attributes
{
    public class UniqueStringIgnoreCase : UniqueAttribute
    {
        public UniqueStringIgnoreCase(Type dbContextType, Type entityType, string? propertyName = null) : base(
            dbContextType, entityType, propertyName)
        {
        }

        public override IEnumerable<LambdaExpression> SourceSelectors { get; set; } =
            new[] {ExpressionUtils.ToLowerString()};

        public override IEnumerable<LambdaExpression> ValueSelectors { get; set; } =
            new[] {ExpressionUtils.ToLowerString()};

        protected override void SaveBatchState(object value, ValidationContext context)
        {
            base.SaveBatchState(((string) value).ToLower(), context);
        }

        protected override bool ValidateBatchState(object value, ValidationContext context)
        {
            return base.ValidateBatchState(((string) value).ToLower(), context);
        }
    }
}