using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TokenBasedAuthApplication.Core.DataAccess.Common;
using TokenBasedAuthApplication.DataAccess.Contexts.EntityFramework;

namespace TokenBasedAuthApplication.DataAccess.Repositories.Common;

public class GenericRepository<TEntity>: IGenericRepository<TEntity> where TEntity : class, new()
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet.FindAsync(Id, cancellationToken);
        if (entity != null) _context.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    public async Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        return await Task.Run(function: () => _dbSet
            .Where(expression)
            .AsNoTracking()
            .AsQueryable(), cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>>? expression = null)
    {
        return expression == null
            ? await _dbSet.AsNoTracking().ToListAsync(cancellationToken)
            : await _dbSet.AsNoTracking().Where(expression).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await Task.Run(function: () => _dbSet.Update(entity), cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await Task.Run(function: () => _dbSet.Remove(entity), cancellationToken);
    }
}