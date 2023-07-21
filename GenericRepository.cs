using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public interface IGenericRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> GetByParameter(Expression<Func<TEntity, bool>> filter);
}

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;

    public GenericRepository(DbContext context)
    {
        _context = context;
    }

    public IEnumerable<TEntity> GetByParameter(Expression<Func<TEntity, bool>> filter)
    {
        return _context.Set<TEntity>().Where(filter).ToList();
    }
}

