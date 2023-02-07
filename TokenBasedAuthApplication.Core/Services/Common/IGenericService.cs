using System.Linq.Expressions;
using TokenBasedAuthApplication.SharedLibrary;
using TokenBasedAuthApplication.SharedLibrary.DTOs;

namespace TokenBasedAuthApplication.Core.Services.Common;

public interface IGenericService<TEntity, TDto> where TEntity: class, new()
{
    Task<Response<TDto>> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
    Task<Response<TDto>> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
    Task<Response<IReadOnlyList<TDto>>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>>? expression = null);
    Task<Response<TDto>> AddAsync(TDto dto, CancellationToken cancellationToken);
    Task<Response<TDto?>> UpdateAsync(TDto dto, Guid Id, CancellationToken cancellationToken);
    Task<Response<NoDataDto>> DeleteAsync(Guid Id, CancellationToken cancellationToken);
}