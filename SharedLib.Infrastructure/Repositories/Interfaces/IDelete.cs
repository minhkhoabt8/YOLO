namespace SharedLib.Infrastructure.Repositories.Interfaces;

public interface IDelete<in T> where T : class
{
    void Delete(T obj);
}