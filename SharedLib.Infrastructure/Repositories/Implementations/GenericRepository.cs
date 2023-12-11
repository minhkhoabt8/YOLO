using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SharedLib.Infrastructure.Repositories.Interfaces;
using SharedLib.Infrastructure.Repositories.QueryExtensions;

namespace SharedLib.Infrastructure.Repositories.Implementations;

public class GenericRepository<TEntity, TContext> :
    IGetAllAsync<TEntity>,
    IFindAsync<TEntity>,
    IAddAsync<TEntity>,
    IUpdate<TEntity>,
    IDelete<TEntity> where TEntity : class where TContext : DbContext
{
    protected readonly TContext _context;

    public GenericRepository(TContext context)
    {
        _context = context;
    }

    public virtual async Task AddAsync(TEntity obj)
    {
        await _context.Set<TEntity>().AddAsync(obj);
    }

    public virtual void Delete(TEntity obj)
    {
        _context.Set<TEntity>().Remove(obj);
    }

    public virtual async Task<TEntity?> FindAsync(object key, string include, bool trackChanges = true)
    {
        IQueryable<TEntity> entries = _context.Set<TEntity>();

        if (!trackChanges)
        {
            entries = entries.AsNoTracking();
        }

        return await entries.IncludeDynamic(include).FirstOrDefaultAsync(GenerateFindByIDExpression(key));
    }

    public virtual async Task<TEntity?> FindIncludeIsActiveAsync(object key, string include, bool trackChanges = true, bool isActive = false)
    {
        IQueryable<TEntity> entries = _context.Set<TEntity>();

        if (!trackChanges)
        {
            entries = entries.AsNoTracking();
        }

        return await entries.IncludeDynamic(include, isActive).FirstOrDefaultAsync(GenerateFindByIDExpression(key));
    }


    public virtual async Task<TEntity?> FindAsync(params object[] keys)
    {
        return await _context.Set<TEntity>().FindAsync(keys);
    }

    public virtual Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false)
    {
        IQueryable<TEntity> dbSet = _context.Set<TEntity>();
        if (trackChanges == false)
        {
            dbSet = dbSet.AsNoTracking();
        }

        return Task.FromResult(dbSet.AsEnumerable());
    }

    public virtual Task<IEnumerable<TEntity>> GetAllAsync(string include, bool trackChanges = false)
    {
        IQueryable<TEntity> dbSet = _context.Set<TEntity>();

        if (!trackChanges)
        {
            dbSet = dbSet.AsNoTracking();
        }

        if (!string.IsNullOrEmpty(include))
        {
            dbSet = dbSet.Include(include);
        }

        return Task.FromResult(dbSet.AsEnumerable());
    }


    public virtual void Update(TEntity obj)
    {
        _context.Set<TEntity>().Update(obj);
    }

    private Expression<Func<TEntity, bool>> GenerateFindByIDExpression(object id)
    {
        var propInfo =
            typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                .SingleOrDefault(prop => prop.Name == "ID" || prop.GetCustomAttribute<KeyAttribute>() != null) ??
            throw new Exception($"Could not find primary key attribute of entity {typeof(TEntity).Name}");

        var objParameterExpr = Expression.Parameter(typeof(TEntity));
        var propertyExpr = Expression.Property(objParameterExpr, propInfo);
        var equalExpr = Expression.Equal(propertyExpr, Expression.Constant(id));
        return Expression.Lambda<Func<TEntity, bool>>(equalExpr, objParameterExpr);
    }

}