using SharedLib.Core.Entities;

namespace SharedLib.Infrastructure.Repositories.QueryExtensions;

public static class SoftDeleteEntityQueryExtensions
{
    public static IQueryable<T> AllowInactive<T>(this IQueryable<T> query, bool? showInactive)
        where T : ISoftDeleteEntity
    {
        return query.Where(e => !showInactive.HasValue || showInactive.Value || e.IsActive);
    }
}