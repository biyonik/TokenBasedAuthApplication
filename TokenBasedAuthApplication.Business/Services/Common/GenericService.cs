using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TokenBasedAuthApplication.Business.Mappings;
using TokenBasedAuthApplication.Core.DataAccess.Common;
using TokenBasedAuthApplication.Core.Services.Common;
using TokenBasedAuthApplication.Core.UnitOfWork;
using TokenBasedAuthApplication.SharedLibrary;
using TokenBasedAuthApplication.SharedLibrary.DTOs;

namespace TokenBasedAuthApplication.Business.Services.Common;

public class GenericService<TEntity, TDto>: IGenericService<TEntity, TDto> where TEntity : class, new()
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<TEntity> _genericRepository;

    public GenericService(IGenericRepository<TEntity> genericRepository, IUnitOfWork unitOfWork)
    {
        _genericRepository = genericRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<TDto>> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        var entity = await _genericRepository.GetByIdAsync(Id, cancellationToken);
        if (entity == null) return Response<TDto>.Fail("Entity not found!", 404, true);
        var mappedEntity = ObjectMapper.Mapper.Map<TDto>(entity);
        return Response<TDto>.Success(mappedEntity, 200);
    }

    public async Task<Response<TDto>> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        var entity =
            await (await _genericRepository.GetAsync(expression, cancellationToken)).FirstOrDefaultAsync(cancellationToken);
        if (entity == null) return Response<TDto>.Fail("Entity not found!", 404, true);
        var mappedEntity = ObjectMapper.Mapper.Map<TDto>(entity);
        return Response<TDto>.Success(mappedEntity, 200);
    }

    public async Task<Response<IReadOnlyList<TDto>>> GetAllAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>>? expression = null)
    {
        var entities = await _genericRepository.GetAllAsync(cancellationToken, expression);
        if (entities.Count == 0) return Response<IReadOnlyList<TDto>>.Fail("Entities not found!", 404, true);
        var mappedEntities = ObjectMapper.Mapper.Map<IReadOnlyList<TDto>>(entities);
        return Response<IReadOnlyList<TDto>>.Success(mappedEntities, 200);
    }

    public async Task<Response<TDto>> AddAsync(TDto dto, CancellationToken cancellationToken)
    {
        if (dto == null) return Response<TDto>.Fail("Entity not found!", 404, true);
        
        var entity = ObjectMapper.Mapper.Map<TEntity>(dto);
        await _genericRepository.AddAsync(entity, cancellationToken);
        var result =  await _unitOfWork.CommitAsync();
        return !result 
            ? Response<TDto>.Fail("Add operation failed!", 400, true) 
            : Response<TDto>.Success(dto, 201);
    }

    public async Task<Response<TDto?>> UpdateAsync(TDto dto, Guid Id, CancellationToken cancellationToken)
    {
        var isExist = await _genericRepository.GetByIdAsync(Id, cancellationToken);
        if (isExist == null) return Response<TDto?>.Fail("Entity not found!", 404, true);
        
        var entity = ObjectMapper.Mapper.Map<TEntity>(dto);
        await _genericRepository.UpdateAsync(entity, cancellationToken);
        var result = await _unitOfWork.CommitAsync();
        return !result 
            ? Response<TDto?>.Fail("Update operation failed!", 400, true) 
            : Response<TDto?>.Success(dto, 204);
    }

    public async Task<Response<NoDataDto>> DeleteAsync(TDto dto, Guid Id, CancellationToken cancellationToken)
    {
        var isExist = await _genericRepository.GetByIdAsync(Id, cancellationToken);
        if (isExist == null) return Response<NoDataDto>.Fail("Entity not found!", 404, true);
        
        var entity = ObjectMapper.Mapper.Map<TEntity>(dto);
        await _genericRepository.DeleteAsync(entity, cancellationToken);
        var result = await _unitOfWork.CommitAsync();
        return !result 
            ? Response<NoDataDto>.Fail("Delete operation failed!", 400, true) 
            : Response<NoDataDto>.Success(200);
    }
}