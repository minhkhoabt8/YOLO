namespace SharedLib.Infrastructure.Repositories.Interfaces;

public interface IQueryAsync<TEntity, in TQuery>
{
    Task<IEnumerable<TEntity>> QueryAsync(TQuery query, bool trackChanges = false);
}