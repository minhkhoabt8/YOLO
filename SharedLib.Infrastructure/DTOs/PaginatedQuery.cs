namespace SharedLib.Infrastructure.DTOs;

public class PaginatedQuery
{
    private int? _pageNumber;
    private int? _maxPageSize;

    public int? PageNumber
    {
        get => _maxPageSize.HasValue && _pageNumber.HasValue ? Math.Max(1, _pageNumber.Value) : 1;
        set => _pageNumber = value;
    }

    public int? MaxPageSize
    {
        get => _maxPageSize.HasValue ? Math.Max(1, _maxPageSize.Value) : int.MaxValue;
        set => _maxPageSize = value;
    }
}