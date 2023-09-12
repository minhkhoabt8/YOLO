namespace SharedLib.Infrastructure.Repositories.Interfaces;

public interface IUpdate<in T> where T : class
{
    void Update(T obj);
}