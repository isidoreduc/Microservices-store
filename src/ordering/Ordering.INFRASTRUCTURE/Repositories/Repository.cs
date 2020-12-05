using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.CORE.Entities;
using Ordering.CORE.Entities.Base;
using Ordering.CORE.Repositories;
using Ordering.INFRASTRUCTURE.Data;

namespace Ordering.INFRASTRUCTURE.Repositories
{
  public class Repository<T> : IRepositoryBase<T> where T:Entity
  {
    protected readonly OrderContext _ctx;
    public Repository(OrderContext ctx)
    {
      _ctx = ctx;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
      return await _ctx.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
      return await _ctx.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
    {
      IQueryable<T> query = _ctx.Set<T>();
      if (disableTracking) query = query.AsNoTracking();

      if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

      if (predicate != null) query = query.Where(predicate);

      if (orderBy != null)
          return await orderBy(query).ToListAsync();
      return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
    {
      IQueryable<T> query = _ctx.Set<T>();
      if (disableTracking) query = query.AsNoTracking();

      if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

      if (predicate != null) query = query.Where(predicate);

      if (orderBy != null)
          return await orderBy(query).ToListAsync();
      return await query.ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
      return await _ctx.Set<T>().FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
      _ctx.Set<T>().Add(entity);
      await _ctx.SaveChangesAsync();
      return entity;
    }

    public async Task UpdateAsync(T entity)
    {
      _ctx.Entry(entity).State = EntityState.Modified;
      await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
      _ctx.Set<T>().Remove(entity);
      await _ctx.SaveChangesAsync();
    }
  }
}