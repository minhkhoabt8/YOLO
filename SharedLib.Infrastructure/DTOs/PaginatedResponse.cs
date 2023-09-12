using System.Diagnostics.CodeAnalysis;
using AutoMapper;

namespace SharedLib.Infrastructure.DTOs;

public class PaginatedResponse<T> : List<T>
{
    public Pagination Pagination { get; }

    public PaginatedResponse(IEnumerable<T> items, int count, int pageNumber, int maxPageSize)
    {
        int totalPages = (int) Math.Ceiling(count / (double) maxPageSize);

        Pagination = new Pagination
        {
            CurrentPage = pageNumber,
            MaxPageSize = maxPageSize,
            TotalPages = totalPages,
            TotalCount = count,
            HasNext = pageNumber < totalPages,
            HasPrevious = pageNumber > 1
        };
        AddRange(items);
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static PaginatedResponse<T> FromEnumerable(IEnumerable<T> source, PaginatedQuery query)
    {
        var count = source.Count();
        var items = source.Skip((query.PageNumber!.Value - 1) * query.MaxPageSize!.Value).Take(query.MaxPageSize!.Value)
            .ToList();

        return new PaginatedResponse<T>(items, count, query.PageNumber!.Value, query.MaxPageSize!.Value);
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static PaginatedResponse<T> FromEnumerableWithMapping<TSource>(IEnumerable<TSource> source,
        PaginatedQuery query, IMapper mapper)
    {
        var count = source.Count();
        var items = source.Skip((query.PageNumber!.Value - 1) * query.MaxPageSize!.Value).Take(query.MaxPageSize!.Value)
            .ToList();

        return new PaginatedResponse<T>(mapper.Map<IEnumerable<T>>(items), count, query.PageNumber!.Value,
            query.MaxPageSize!.Value);
    }
}

public class Pagination
{
    public int TotalCount { get; set; }
    public int MaxPageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}