using Application.Abstractions;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class Repository<TContext, TEntity> : IRepository<TEntity> where TContext : DbContext where TEntity : BaseEntity, new()
{
    private readonly TContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(TContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbSet.UpdateRange(entities);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
        }
        _dbSet.UpdateRange(entities);
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var query = GenerateQuery(predicate, include, orderBy, asNoTracking);

        return await query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var query = GenerateQuery(predicate, include, orderBy, asNoTracking);

        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool asNoTracking = false)
    {
        IQueryable<TEntity> queryable = _dbSet;
        if (predicate is not null)
            queryable = queryable.Where(predicate);

        if (orderBy is not null)
            queryable = orderBy(queryable);

        if (include is not null)
            queryable = include(queryable);

        return !asNoTracking ? queryable : queryable.AsNoTracking();
    }

    #region helper

    private IQueryable<TEntity> GenerateQuery(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, bool asNoTracking = false)
    {
        IQueryable<TEntity> queryable = _dbSet;
        if (predicate is not null)
        {
            queryable = queryable.Where(predicate);
        }

        if (orderBy is not null)
        {
            queryable = orderBy(queryable);
        }

        if (include is not null)
        {
            queryable = include(queryable);
        }

        return asNoTracking ? queryable.AsNoTracking() : queryable;
    }

    #endregion
}
