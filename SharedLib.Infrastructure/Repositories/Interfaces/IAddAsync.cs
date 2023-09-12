namespace SharedLib.Infrastructure.Repositories.Interfaces;

public interface IAddAsync<in T> where T : class
{
    Task AddAsync(T obj);
}