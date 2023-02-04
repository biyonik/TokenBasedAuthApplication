using System.Linq.Expressions;

namespace TokenBasedAuthApplication.Core.DataAccess.Common;

public interface IGenericRepository<T> where T: class, new()
{
    Task<T?> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<T, bool>>? expression = null);
    Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
    Task<bool> AddAsync(T entity, CancellationToken cancellationToken);
    Task<T?> UpdateAsync(T entity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken);
}