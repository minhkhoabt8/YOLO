namespace SharedLib.Infrastructure.Repositories.Interfaces;

public interface IFindAsync<T> where T : class
{
    Task<T?> FindAsync(object key, string include, bool trackChanges = true);
    Task<T?> FindIncludeIsActiveAsync(object key, string include, bool trackChanges = true, bool isActive = false);
    Task<T?> FindAsync(params object[] keys);
}