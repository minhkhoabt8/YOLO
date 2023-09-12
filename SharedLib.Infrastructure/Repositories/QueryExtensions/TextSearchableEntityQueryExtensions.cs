using FuzzySharp;
using SharedLib.Core.Entities;

namespace SharedLib.Infrastructure.Repositories.QueryExtensions;

public static class TextSearchableEntityQueryExtensions
{
    public static IQueryable<T> FilterAndOrderByTextSimilarity<T>(this IQueryable<T> query, string searchText,
        double minSimiliarity = 0) where T : ITextSearchableEntity
    {
        return query.AsEnumerable()
            .Select(e => new
            {
                Entity = e,
                Similarity = e.SearchTextsWithWeights.Sum(kv =>
                    kv.Value * Fuzz.PartialRatio(searchText.ToLower(), kv.Key.Invoke().ToLower()))
            }).Where(es => es.Similarity >= minSimiliarity).OrderBy(es => -es.Similarity).Select(es => es.Entity)
            .AsQueryable();
    }
}