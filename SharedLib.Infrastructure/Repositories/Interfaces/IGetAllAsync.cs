namespace SharedLib.Infrastructure.Repositories.Interfaces;

public interface IGetAllAsync<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false);
    Task<IEnumerable<T>> GetAllAsync(string include, bool trackChanges = false);
}