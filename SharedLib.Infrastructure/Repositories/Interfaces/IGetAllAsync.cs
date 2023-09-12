namespace SharedLib.Infrastructure.Repositories.Interfaces;

public interface IGetAllAsync<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false);
}