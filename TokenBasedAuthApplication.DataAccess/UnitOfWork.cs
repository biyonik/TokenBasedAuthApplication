using Microsoft.EntityFrameworkCore;
using TokenBasedAuthApplication.Core.UnitOfWork;
using TokenBasedAuthApplication.DataAccess.Contexts.EntityFramework;

namespace TokenBasedAuthApplication.DataAccess;

public class UnitOfWork: IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CommitAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public bool Commit()
    {
        return _context.SaveChanges() > 0;
    }
}